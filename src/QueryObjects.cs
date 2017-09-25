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
	
	public class QueryObjects<T> {
		
		public static async Task<List<T>> get(QueryResult result){
			List<T> res = new List<T>();
			
			IEntityManager em = EntityManager.getInstance();
			EntityRepository repo = em.getRepository(result.entity_repository_name);
			
			while (await result.data.ReadAsync()){
				Type entity_type = repo.getEntityType();
				object obj = Activator.CreateInstance(repo.getEntityType());
				((IEntity)obj).assignData(result.data);
				res.Add((T)obj);
			}
			
			return res;
		}
	}
	
}