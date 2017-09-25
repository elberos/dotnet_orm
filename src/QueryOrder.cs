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
	
	public class QueryOrder{
		
		public static readonly long ASC = 1;
	    public static readonly long DESC = -1;
		
		public string name = "";
		public int direction = 1;
		
		
		public QueryOrder(string name, int direction){
			this.name = name;
			this.direction = direction;
		}
		
	}
	
}
	