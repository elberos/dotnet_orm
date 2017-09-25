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

namespace Elberos.Orm{
	
	public class QueryResult {
		
		public Type entity_repository_name = null;
		public QueryBuilder qb = null;
		public DbDataReader data = null;
		public long page = 0;
		public long start = 0;
		public long limit = 0;
		protected long _pages = -1;
		protected long _count = -1;
		
		
		public long count(){
			if (this._count == -1)
				if (this.qb != null)
					this._count = this.qb.foundRows();
			return this._count;
		}
		
		public long pages(){
			if (this._pages == -1)
				if (this.qb != null)
					this._pages = this.qb.getPages();
			return this._pages;
		}
		
	}
}