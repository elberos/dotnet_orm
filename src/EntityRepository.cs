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

namespace Elberos.Orm{
	
	public abstract class EntityRepository{
		
		protected Dictionary<string, EntityFieldInfo> _struct = null;
		
		
		/**
		 * Returns Entity type
		 * 
		 * @return Type
		 */
		public abstract Type getEntityType();
		
		
		/**
		 * Returns table name of the this entity
		 *
		 * @return string
		 */
	    public abstract string getTableName();
		
		
		/**
		 * Build struct fields
		 *
		 * @param StructBuilder $builder
		 */
	    public abstract void buildStruct(EntityStructBuilder builder);
		
		
		
		public Dictionary<string, EntityFieldInfo> getStruct(){
			
			if (this._struct == null){
				EntityStructBuilder builder = new EntityStructBuilder();
				this.buildStruct(builder);
				this._struct = builder.getStruct();
			}
			
			return this._struct;
		}
		
		
		
		/**
		 * Create Instance of class by Dictionary<string, dynamic>
		 */
		public Entity Instance(Dictionary<string, dynamic> data = null){
			
			Type t = this.getEntityType();
			Entity e = (Entity)Activator.CreateInstance(t);
			
			e.assignData(data);
			e.postLoad();
			
			return e;
		}
		
		
		/**
		 * Create Instance of class by DbDataReader
		 */
		public Entity Instance(DbDataReader res = null){
			
			Type t = this.getEntityType();
			Entity e = (Entity)Activator.CreateInstance(t);
			
			e.assignData(res);
			e.postLoad();
			
			return e;
		}
		
		
	}
	
}