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
	
	public class EntityFieldInfo{
		
		protected bool _is_primary = false;
		protected bool _is_auto = false;
		protected bool _is_null = false;
		protected string _field_name = "";
		protected string _title = "";
		protected Type _db_type = null;
		protected Type _web_type = null;
		protected string _comment = null;
		protected dynamic _default_value = null;
		
		public EntityFieldInfo(Dictionary<string, dynamic> info){
			foreach (KeyValuePair<string, dynamic> elem in info){
				if (elem.Key == "primary") this._is_primary = (bool) elem.Value;
				else if (elem.Key == "auto") this._is_auto = (bool) elem.Value;
				else if (elem.Key == "nullable") this._is_null = (bool) elem.Value;
				else if (elem.Key == "name") this._field_name = (string) elem.Value;
				else if (elem.Key == "title") this._title = (string) elem.Value;
				else if (elem.Key == "dbtype") this._db_type = (Type) elem.Value;
				else if (elem.Key == "webtype") this._web_type = (Type) elem.Value;
				else if (elem.Key == "comment") this._comment = (string) elem.Value;
				else if (elem.Key == "default") this._default_value = elem.Value;
			}
		}
		
		public bool isPrimary(){
			return this._is_primary;
		}
		
		public bool isAuto(){
			return this._is_auto;
		}
		
		public bool isNull(){
			return this._is_null;
		}
		
		public string fieldName(){
			return this._field_name;
		}
		
		public string title(){
			return this._title;
		}
		
		public Type dbType(){
			return this._db_type;
		}
		
		public bool isDbType(){
			return this._db_type != null;
		}
		
		public Type webType(){
			return this._web_type;
		}
		
		public bool isWebType(){
			return this._web_type != null;
		}
		
		public string comment(){
			return this._comment;
		}
		
		public dynamic defaultValue(){
			return this._default_value;
		}
		
	}
	
}