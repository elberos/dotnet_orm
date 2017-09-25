/**
 * This file is part of the Elberos package.
 *
 * (c) Ildar Bikmamatov <elberos@bayrell.org>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */
 
using System;
using System.Collections.Generic;

namespace Elberos.Orm{
	
	public class QueryFilter{
		
		public static readonly string TYPE_OR = "or";
		public static readonly string TYPE_AND = "and";
		public static readonly string TYPE_IN = "in";
		public static readonly string TYPE_EQUAL = "=";
		public static readonly string TYPE_NOT_EQUAL = "!=";
		
		
		public string type = "";
		public string key = "";
		public dynamic value = "";
		
		public QueryFilter(string key, string type, dynamic value){
			this.key = key;
			this.type = type;
			this.value = value;
		}
		
	}
	
}
	