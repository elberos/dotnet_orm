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
	
	public class EntityStructBuilder{
		
		public Dictionary<string, EntityFieldInfo> _struct = null;
		
		public Dictionary<string, EntityFieldInfo> getStruct(){
			return this._struct;
		}
		
		public EntityStructBuilder(){
			this._struct = new Dictionary<string, EntityFieldInfo>();
		}
		
		public void addField(EntityFieldInfo field){
			string name = field.fieldName();
			this._struct.Add(name, field);
		}
		
		public void addFields(List<EntityFieldInfo> fields){
			foreach(EntityFieldInfo field in fields){
				string name = field.fieldName();
				this._struct.Add(name, field);
			}
		}
			
	}
}