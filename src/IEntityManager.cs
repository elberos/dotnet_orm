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
	
	public interface IEntityManager{
		
		void setConnections(IConnections connections);
		IConnections getConnections();
		Connection getConnection(string connection_name);
		void getStruct(dynamic class_name);
		QueryBuilder createQueryBuilder(string connection_name);
		EntityRepository getRepository(dynamic entity_repository_name);
		EntityBaseType getEntityType(dynamic class_name);
		QueryBuilder select(dynamic entity_repository_name, string[] fields = null, 
				string alias_name = null, string connection_name = null);
		Task<Entity> findOne(dynamic entity_repository_name, 
				List<QueryFilter> filter, string connection_name = null);
		Task<Entity> findById(dynamic entity_repository_name, dynamic id, 
				string connection_name = null);	
		Task<Entity> getById(dynamic entity_repository_name, dynamic id, 
				string connection_name = null);					
	}
}