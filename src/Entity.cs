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
using System.Collections.Generic;
using System.Reflection;

namespace Elberos.Orm{
	
	public abstract class Entity : IEntity{
		
		protected bool _is_new = true;
		protected Dictionary<string, dynamic> _old_data = null;
		protected string _connection_name = null;
		
		
		/**
		 * Returns Entity repository
		 * 
		 * @return Type
		 */
		public abstract Type getEntityRepositoryType();
		
		
		/**
		 * Get connection name of this entity
		 *
		 * @return string
		 */
		public virtual string getConnectionName(){
			return this._connection_name;
		}
		
		
		/**
		 * Set connection name of this entity
		 *
		 * @param string new_connection_name
		 */
		public virtual void setConnectionName(string new_connection_name){
			this._connection_name = new_connection_name;
		}
		
		
		/**
		 * Save Entity
		 */
		public virtual void save(bool force = false){
			if (this._is_new || force) this.create();
			else this.update();
		}
			
			
		/**
		 * Create Entity
		 */
		public virtual async void create(){
			this.preCreate();
			
			string auto_field_name = null;
			IEntityManager em = EntityManager.getInstance();
			Connection connection = em.getConnection(this.getConnectionName());
			QueryBuilder qb = em.createQueryBuilder(this.getConnectionName());
			EntityRepository repo = em.getRepository(this.getEntityRepositoryType());
			Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
			
			//qb.
			
			Dictionary<string, EntityFieldInfo> st = repo.getStruct();
			foreach (EntityFieldInfo field in st.Values){
				if (!field.isDbType())
					continue;
				
				string field_name = field.fieldName();
				dynamic value = this.getDatabaseValue(field, connection);
				
				if (field.isPrimary() && field.isAuto() && value != null){
					auto_field_name = field_name;
					continue;
				}
				
				data[field_name] = value;
				qb.set(field_name, value);
			}
			
			// Run query
			await qb.execute();
			
			if (auto_field_name != null){
				dynamic id = qb.lastInsertId();
				this.setValue(auto_field_name, id);
				data[auto_field_name] = id;
			}
			
			this._is_new = false;
			this._old_data = data;
			this.postCreate();
		}
			
			
		/**
		 * Update Entity
		 */
		public virtual async void update(){
			this.preUpdate();
			
			bool changed = false;
			IEntityManager em = EntityManager.getInstance();
			Connection connection = em.getConnection(this.getConnectionName());
			QueryBuilder qb = em.createQueryBuilder(this.getConnectionName());
			EntityRepository repo = em.getRepository(this.getEntityRepositoryType());
			Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
			
			Dictionary<string, EntityFieldInfo> st = repo.getStruct();
			foreach (EntityFieldInfo field in st.Values){
				if (!field.isDbType())
					continue;
					
				string field_name = field.fieldName();
				dynamic value = this.getDatabaseValue(field, connection);
				dynamic old_value = this._old_data.ContainsKey(field_name) ? 
											this._old_data[field_name] : null;
				
				// If primary
				if (field.isPrimary()){
					qb.addFilter(new QueryFilter(field_name, "=", old_value));
				}
					
				if (old_value != value){
					changed = true;
					qb.set(field_name, value);
				}
				
				data[field_name] = value;
			}
			
			if (changed){
				await qb.execute();
			}
			
			this._is_new = false;
			this._old_data = data;
			this.postUpdate();
		}
		
		
		/**
		 * Delete Entity
		 */
		public virtual async void remove(){
			this.preRemove();
			
			bool execute = false;
			IEntityManager em = EntityManager.getInstance();
			Connection connection = em.getConnection(this.getConnectionName());
			QueryBuilder qb = em.createQueryBuilder(this.getConnectionName());
			EntityRepository repo = em.getRepository(this.getEntityRepositoryType());
			
			Dictionary<string, EntityFieldInfo> st = repo.getStruct();
			foreach (EntityFieldInfo field in st.Values){
				if (!field.isDbType())
					continue;
					
				string field_name = field.fieldName();
				dynamic old_value = this._old_data.ContainsKey(field_name) ? 
											this._old_data[field_name] : null;
				
				// If primary
				if (field.isPrimary()){
					execute = true;
					qb.addFilter(new QueryFilter(field_name, "=", old_value));
				}
			}
			
			// Execute query
			if (execute){
				await qb.execute();
			}
			
			this.postRemove();
		}
		
		
		/**
		 * Delete Entity
		 */
		public virtual void delete(){
			this.remove();
		}
		
		
		/**
		 * Get database value
		 *
		 * @param EntityFieldInfo field
		 * @param Connection connection
		 * @return dynamic
		 */
		public virtual dynamic getDatabaseValue(EntityFieldInfo field, Connection connection){
			IEntityManager em = EntityManager.getInstance();
			string field_name = field.fieldName();
			Type db_type = field.dbType();
			bool is_null = field.isNull();
			
			dynamic value = this.getValue(field_name);
			if (value is EntityBaseType){
				value = value.value();
			}
			
			EntityBaseType obj = em.getEntityType(db_type);
			if (obj != null){
				value = obj.convertToDatabase(value, connection);
				if (value == null && !is_null) value = obj.getEmptyValue();
			}
			
			return value;
		}
		
		
		/**
		 * Set database value
		 *
		 * @param EntityFieldInfo field
		 * @param Connection connection
		 * @param dynamic db_value
		 */
		public virtual void setDatabaseValue(EntityFieldInfo field, Connection connection,
				 dynamic db_value){
			IEntityManager em = EntityManager.getInstance();
			string field_name = field.fieldName();
			Type db_type = field.dbType();
			bool is_null = field.isNull();
			
			dynamic obj_value = this.getValue(field_name);
			EntityBaseType obj = em.getEntityType(db_type);
			if (obj != null){
				db_value = obj.convertFromDatabase(db_value, connection);
				if (obj_value is EntityBaseType){
					obj_value.assign(db_value);
				}
				else{
					this.setValue(field_name, db_value);
				}
			}
		}
		
		
		/**
		 * Get entity value
		 *
		 * @param string field_name
		 * @param dynamic default_value
		 * @return dynamic
		 */
		public virtual dynamic getValue(string field_name, dynamic default_value = null){
			PropertyInfo property = this.GetType().GetProperty(field_name);
        	return property.GetValue(this, null);
		}
		
		
		/**
		 * Set entity value
		 *
		 * @param string field_name
		 * @param dynamic value
		 * @return dynamic
		 */
		public virtual void setValue(string field_name, dynamic value = null){
			PropertyInfo property = this.GetType().GetProperty(field_name);
        	property.SetValue(this, value, null);
		}
		
		
		/**
		 * Assign values
		 * @param Dictionary<string, dynamic> data
		 */
		public virtual void assignData(Dictionary<string, dynamic> data){
			foreach(KeyValuePair<string, dynamic> pair in data){
				this.setValue(pair.Key, pair.Value);
			}
		}
		
		
		/**
		 * Assign values from database
		 * @param DbDataReader data
		 */
		public virtual void assignData(DbDataReader res){
			IEntityManager em = EntityManager.getInstance();
			Connection connection = em.getConnection(this.getConnectionName());
			EntityRepository repo = em.getRepository(this.getEntityRepositoryType());
			
			Dictionary<string, EntityFieldInfo> st = repo.getStruct();
			foreach (EntityFieldInfo field in st.Values){
				if (!field.isDbType())
					continue;
					
				string field_name = field.fieldName();
				int pos = res.GetOrdinal(field_name);
				dynamic db_value = res.GetFieldValue<dynamic>(pos);
				this.setDatabaseValue(field, connection, db_value);
			}
		}
		
		
		/**
		 * Events
		 */
		public virtual void preCreate(){
		}
		
		public virtual void postCreate(){
		}
		
		public virtual void preUpdate(){
		}
		
		public virtual void postUpdate(){
		}
		
		public virtual void preRemove(){
		}
		
		public virtual void postRemove(){
		}
		
		public virtual void postLoad(){
		}
		
		public virtual void onInit(){
		}
	}
}