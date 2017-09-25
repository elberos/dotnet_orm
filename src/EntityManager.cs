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
	
	public class EntityManager : IEntityManager{
		
		protected static IEntityManager _instance = null;
		
		public static IEntityManager getInstance(){
			return EntityManager._instance;
		}
		public static void setInstance(IEntityManager em){
			EntityManager._instance = em;
		}
		
		protected Dictionary<Type, object> types = null;
		protected IConnections connections = null;
		
		public EntityManager(IConnections connections){
			this.connections = connections;
			this.types = new Dictionary<Type, object>();
		}
		
		public virtual void setConnections(IConnections connections){
			this.connections = connections;
		}
		
		public virtual IConnections getConnections(){
			return this.connections;
		}
		
		public virtual object getEntityType(dynamic class_name){
			if (class_name is string){
				class_name = Type.GetType(class_name);
			}
			if (!(class_name is Type)){
				return null;
			}
			if (this.types.ContainsKey(class_name)){
				return this.types[class_name];
			}
			
			object obj = Activator.CreateInstance(class_name);
			if (obj is EntityBaseType){
				this.types.Add(class_name, obj);
				return obj;
			}
			
			return null;
		}
		
		public Connection getConnection(string connection_name){
			return this.connections.get(connection_name);
		}
		
		
		public QueryBuilder createQueryBuilder(string connection_name){
			Connection connection = this.getConnection(connection_name);
			return connection.createQueryBuilder();
		}
		
		
		/**
		 * Get Entity repository
	     *
	     * @return EntityRepository
		 */
		public EntityRepository getRepository(dynamic entity_repository_name){
			if (entity_repository_name is string){
				entity_repository_name = Type.GetType(entity_repository_name);
			}
			
			return null;
		}
		
		
		
		/**
		 * Get Entity struct
	     *
	     * @return array
		 */
		public virtual void getStruct(dynamic class_name){
			if (class_name is string){
				class_name = Type.GetType(class_name);
			}
			
			if (class_name is Type){
			}
		}
		
		
		
		/**
		 * Return select query builder
	     *
	     * @return array
		 */
		public virtual QueryBuilder select(dynamic entity_repository_name, string[] fields = null, 
				string alias_name = null, string connection_name = null){
			
			if (entity_repository_name is string){
				entity_repository_name = Type.GetType(entity_repository_name);
			}
			
			Connection connection = this.connections.get(connection_name);
			QueryBuilder qb = connection.createQueryBuilder();
			
			qb.select(fields);
			qb.setAlias(alias_name);
			qb.setEntityRepository(entity_repository_name);
			
			return qb;
		}
		
		
		
		/**
		 * Find one Entity by filter
	     *
	     * @return Task<Entity>
		 */
		public virtual async Task<Entity> findOne(dynamic entity_repository_name, 
				List<QueryFilter> filter, string connection_name = null){
			
			QueryBuilder qb = this.select(entity_repository_name, null, null, connection_name);
			qb.filter(filter);
			qb.limit(1);
			await qb.execute();
			
			DbDataReader res = qb.getRawResult();
			if (await res.ReadAsync()){
				EntityRepository repository = this.getRepository(entity_repository_name);
				Entity entity = repository.Instance(res);
				return entity;
			}
			
			return null;
		}
		
		
		
		/**
		 * Find Entity by id
	     *
	     * @return array
		 */
		public virtual  async Task<Entity> findById(dynamic entity_repository_name, dynamic id, 
				string connection_name = null){
			
			List<QueryFilter> filter = new List<QueryFilter>();
			filter.Add(new QueryFilter("id", "=", id));
			
			return await this.findOne(entity_repository_name, filter, connection_name);
		}
		
		
		/**
		 * Get Entity by id
	     *
	     * @return array
		 */
		public virtual  async Task<Entity> getById(dynamic entity_repository_name, dynamic id, 
				string connection_name = null){
			return await this.findById(entity_repository_name, id, connection_name);
		}
		
		
	}
}