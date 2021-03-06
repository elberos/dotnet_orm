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
	
	public class EntityStringType : EntityBaseType{
		
		protected long _value = 0;
		
		public override dynamic convertToDatabase(dynamic value, Connection connection){
			return connection.convertStringToDatabase(value);
		}
		public override dynamic convertFromDatabase(dynamic value, Connection connection){
			return connection.convertStringFromDatabase(value);
		}
		public override dynamic getEmptyValue(){
			return 0;
		}
		
		public override dynamic value(){
			return this._value;
		}
		public override void assign(dynamic value){
			this._value = value;
		}
		
		
		public EntityStringType(Entity entity, string field_name) : base(entity, field_name) {
		}
		
	}
}