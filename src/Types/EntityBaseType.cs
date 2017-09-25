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
	
	public abstract class EntityBaseType{
		
		protected Entity _entity = null;
		protected string _field_name = null;
		
		public abstract dynamic convertToDatabase(dynamic value, Connection connection);
		public abstract dynamic convertFromDatabase(dynamic value, Connection connection);
		public abstract dynamic getEmptyValue();
		
		public abstract dynamic value();
		public abstract void assign(dynamic value);
		
		public EntityBaseType(Entity entity, string field_name){
			this._entity = entity;
			this._field_name = field_name;
		}
		
	}
}