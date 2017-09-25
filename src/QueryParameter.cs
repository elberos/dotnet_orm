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
	
	public class QueryParameter{
		public string key = "";
		public dynamic value = "";
		
		
		public QueryParameter(string key, dynamic value){
			this.key = key;
			this.value = value;
		}
		
	}
	
}
	