/**
 * This file is part of the Elberos package.
 *
 * (c) Ildar Bikmamatov <elberos@bayrell.org>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Elberos.Orm{
	
	public abstract class QueryBuilder {
		
		/* The query types. */
	    public static readonly long QUERY_NONE = 0;
	    public static readonly long QUERY_SELECT = 1;
	    public static readonly long QUERY_DELETE = 2;
	    public static readonly long QUERY_UPDATE = 3;
		public static readonly long QUERY_INSERT = 4;
		public static readonly long QUERY_INSERT_OR_UPDATE = 5;
		
		
		/**
	     * The type of query this is. Can be select, update or delete.
	     *
	     * @var longeger
	     */
	    protected long _query_type = 0;
		
		
		/**
		 * Current orm connection
		 */
		protected Connection _connection = null;
		
		
		/**
		 * Query alias
		 */
		protected string _alias = "t";
		
		
		/**
		 * Start and Limit
		 */
		protected long _start = 0;
		protected long _limit = -1;
		protected long _page = -1;
		
		protected QueryResult _query_result = null;
		protected Type _entity_repository_name = null;
		protected int _found_rows = -1;
		
		protected List<QueryFilter> _filter = null;
		
		
		/**
		 * Constructor
		 */
		public QueryBuilder(Connection connection){
	        this._connection = connection;
	    }
		
		
		/**
		 * Get orm conntection
		 */
		public virtual Connection getConnection(){
			return this._connection;
		}
		
		
		
		/**
	     * Gets the type of the currently built query.
	     *
	     * @return longeger
	     */
	    public virtual long getQueryType(){
	        return this._query_type;
	    }
		
		
		
		/**
		 * Can use Alias
		 *
		 * @return boolean
		 */
		public virtual bool canAlias(){
			return this._query_type == QueryBuilder.QUERY_SELECT && this._alias != null;
		}
		
		
		
		/**
		 * Set alias name
		 *
		 * @param mixed alias_name Alias name
		 */
		public virtual QueryBuilder setAlias(string alias_name = null){
			if (alias_name != null)
				this._alias = alias_name;
			return this;
		}
		
		
		
		/**
		 * Get field name as "alias.field_name"
		 *
		 * @param string field_name The field_name
		 * @return string field_name with synonym
		 */
		public virtual string getFieldName(string field_name){
			return field_name;
		}
		
		
		/**
		 * Set entity name
		 *
		 * @param string class_name The class name.
		 * @return self
		 */
		public virtual QueryBuilder setEntityRepository(dynamic entity_repository_name){
			
			if (entity_repository_name is string){
				entity_repository_name = Type.GetType(entity_repository_name);
			}
			if (entity_repository_name is Type){
				this._entity_repository_name = entity_repository_name;
				
				// Check if entity_repository_name is subclass of EntityRepository
				if (!((Type)entity_repository_name).IsSubclassOf(typeof(EntityRepository))){
					throw new System.Exception("entity_repository_name '" + 
						(string)entity_repository_name + "' must be EntityRepository");
				}
			}
			else {
				throw new System.Exception("entity_repository_name '" + 
					(string)entity_repository_name + "' must be Type");
			}
			
			return this;
		}
		
		
		/**
		 * Get table name from protected property this._entity_repository_name
		 *
		 * @return string field_name with synonym
		 */
		public virtual string getTableName(){	
			IEntityManager em = EntityManager.getInstance();
			EntityRepository repo = em.getRepository(this._entity_repository_name);
			string table_name = repo.getTableName();
			return table_name;
		}

		
		
		/**
		 * Build select query
		 *
		 * @param array fields List of the fields
		 * @return self
		 */
		public abstract QueryBuilder select(string[] fields = null);
		
		
		
		/**
		 * Build insert query
		 *
		 * @param dynamic entity_repository_name 
		 * @param string alias 
		 * @return self
		 */
		public abstract QueryBuilder insert(dynamic entity_repository_name, string alias = null);
		
		
		
		/**
		 * Build update query
		 *
		 * @param dynamic entity_repository_name 
		 * @param string alias 
		 * @return self
		 */
		public abstract QueryBuilder update(dynamic entity_repository_name, string alias = null);
		
		
		
		/**
		 * Build insert or update query
		 *
		 * @param dynamic entity_repository_name 
		 * @param string alias 
		 * @return self
		 */
		public abstract QueryBuilder insertOrUpdate(dynamic entity_repository_name, string alias = null);
		
		
		
		/**
		 * Build delete query
		 *
		 * @param dynamic entity_repository_name 
		 * @param string alias 
		 * @return self
		 */
		public abstract QueryBuilder delete(dynamic entity_repository_name, string alias = null);
		
		
		
		/**
		 * Set table name
		 *
		 * @param dynamic entity_repository_name 
		 * @param string alias 
		 * @return self
		 */
		public abstract QueryBuilder from(dynamic entity_repository_name, string alias = null);
		
		
		
		/**
	     * Add field to select query
	     *
	     * @param string field The field
	     * @return self
	     */
	    public abstract QueryBuilder addSelect(string field = null);
		
		
		
		/**
	     * Adds a distinct flag of the select query
		 *
		 * @param bool flag
		 * @return self
	     */
	    public abstract QueryBuilder distinct(bool flag = true);
		
		
		
		/**
	     * Adds a SQL_CALC_FOUND_ROWS flag of the select query
		 *
		 * @param bool flag
		 * @return self
	     */
	    public abstract QueryBuilder calcFoundRows(bool flag = true);
		
		
		
		/**
		 * Set new field to query
		 *
		 * @param string key The key
	     * @param string value The value
		 * @return self
		 */
		public abstract QueryBuilder set(string key, dynamic value);
		
		
		
		/**
		 * Set new field to insert query
		 *
		 * @param string key The key
	     * @param string value The value
		 * @return self
		 */
		public abstract QueryBuilder setInsert(string key, dynamic value);
		
		
		
		/**
		 * Set new field to update query
		 *
		 * @param string key The key
	     * @param string value The value
		 * @return self
		 */
		public abstract QueryBuilder setUpdate(string key, dynamic value);
		
		
		
		/**
		 * Set new raw field to query
		 *
		 * @param string key The key
	     * @param string value The value
		 * @return self
		 */
		public abstract QueryBuilder setRaw(string key, dynamic value);
		
		
		
		/**
		 * Set new raw field to insert query
		 *
		 * @param string key The key
	     * @param string value The value
		 * @return self
		 */
		public abstract QueryBuilder setInsertRaw(string key, dynamic value);
		
		
		
		/**
		 * Set new raw field to update query
		 *
		 * @param string key The key
	     * @param string value The value
		 * @return self
		 */
		public abstract QueryBuilder setUpdateRaw(string key, dynamic value);
		
		
		
		/**
		 * Set new values
		 *
		 * @param array arr The array of key => value
		 * @return self
		 */
		public abstract QueryBuilder setValues(Dictionary<string, dynamic> arr);
		
		
		
		/**
		 * Set order
		 *
		 * @param array arr Order list
		 * @return self
		 */
		public abstract QueryBuilder order(Dictionary<string, long> arr);
		
		
		
		/**
	     * Set start position of the first result
	     *
	     * @param longeger start
	     * @return self
	     */
	    public virtual QueryBuilder start(long start){
			if (start >= 0){
				this._start = start;
	        }
	        return this;
	    }
	    
	    
	    
	    /**
	     * Get start position of the first result
	     *
	     * @return longeger
	     */
	    public virtual long getStart(){
	        return this._start;
	    }
		
		
		
		/**
	     * Set maximum number of the results
		 *
	     * @param longeger limit
	     * @return self
	     */
	    public virtual QueryBuilder limit(long limit){
			this._limit = limit;
			this.calcStartLimit();
	        return this;
	    }
		
		
		
		/**
	     * Get maximum number of the results
	     *
	     * @return longeger 
	     */
	    public virtual long getLimit(){
	        return this._limit;
	    }
		
		
		
		/**
	     * Sets the page of the result to retrieve
	     *
	     * @param longeger page The page of the result
	     * @return self
	     */
	    public virtual QueryBuilder page(long page){
			this._page = page;
			this.calcStartLimit();
	        return this;
	    }
	    
	    
	    
	    /**
	     * Gets the page of the result to retrieve
	     *
	     * @return longeger The page of the result.
	     */
	    public virtual long getPage(){
	        return this._page;
	    }
		
		
		
		/**
	     * Calc start and limit
		 *
		 * @return self
	     */
	    public virtual QueryBuilder calcStartLimit(){
			long page = this.getPage();
			long limit = this.getLimit();
			if (page >= 0 && limit >= 0){
				this.start( page * limit );
			}
	        return this;
	    }
		
		
		
		/**
		 * Run query and get data
		 *
		 * @return self
		 */
		public virtual Task<QueryBuilder> execute(){
			return null;
		}
		
		
		
		/**
		 * Return result of the query as Entity objects
		 *
		 * @return array
		 */
		//public getObjects(){
		//	res = this.getQueryResult();
		//	return res.getObjects();
		//}
		
		
		
		/**
		 * Return result of the query
		 *
		 * @return QueryResult
		 */
		public virtual QueryResult getResult(){
			return this.getQueryResult();
		}
		
		
		
		/**
		 * Return raw result
		 *
		 * @return Statement
		 */
		public abstract DbDataReader getRawResult();
		
		
		
		/**
		 * Return result of the query
		 *
		 * @return QueryResult
		 */
		public virtual QueryResult getQueryResult(){
			if (this._query_result != null)
				return this._query_result;
			
			this._query_result = new QueryResult();
			this._query_result.entity_repository_name = this._entity_repository_name;
			this._query_result.qb = this;
			this._query_result.data = this.getRawResult();
			this._query_result.page = this.getCalcPage();
			this._query_result.start = this.getStart();
			this._query_result.limit = this.getLimit();
			
			return this._query_result;
		}
		
		
		
		/**
	     * Get found rows
	     *
	     * @return long 
	     */
	    public abstract long foundRows();
		
		
		
		/**
	     * Get last insert id
	     *
	     * @return long 
	     */
	    public abstract dynamic lastInsertId();
		
		
		
		/**
	     * Get found pages
	     *
	     * @return long 
	     */
	    public virtual long getPages(){
			long rows = this.foundRows();
			long limit = this.getLimit();
			
			if (limit == 0)
				return 0;
			
			return (long)Math.Ceiling((double)(rows / limit));
	    }
		
		
		
		/**
	     * Get page
	     *
	     * @return long 
	     */
	    public virtual long getCalcPage(){
			long start = this.getStart();
			long limit = this.getLimit();
			
			if (limit <= 0)
				return 0;
			
			return (long)Math.Ceiling((double)((start + 1) / limit)) - 1;
	    }
		
		
		
		/**
		 * Set filter
		 */
		public virtual QueryBuilder filter(List<QueryFilter> filter){
			
			// Get where by recurse
			this._filter = filter;
			
			return this;
		}
		
		
		
		/**
		 * Set filter
		 */
		public virtual QueryBuilder addFilter(QueryFilter filter){
			
			// Get where by recurse
			this._filter.Add(filter);
			
			return this;
		}
		
		
		
		/**
		 * Filter or
		 */
		public virtual QueryBuilder addOr(dynamic filter){
			
			// Get where by recurse
			//this._filter[] = ['or', filter];
			
			return this;
		}
		
		
		
		/**
		 * Filter and
		 */
		public virtual QueryBuilder addAnd(dynamic filter){
			
			// Get where by recurse
			//this._filter[] = ['and', filter];
			
			return this;
		}
		
		
		
		/**
		 * Get SQL query
		 *
		 * @return string
		 */
		public abstract dynamic getQuery();
		
	}
}