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
using System.Reflection;

namespace Elberos.Orm{
	
	public interface IEntity{
		
		Type getEntityRepositoryType();
		string getConnectionName();
		void setConnectionName(string new_connection_name);
		void save(bool force = false);
		void create();
		void update();
		void remove();
		void delete();
		dynamic getValue(string field_name, dynamic default_value = null);
		void setValue(string field_name, dynamic value = null);
		dynamic getDatabaseValue(EntityFieldInfo field, Connection connection);
		void setDatabaseValue(EntityFieldInfo field, Connection connection, dynamic db_value);
		void assignData(Dictionary<string, dynamic> data);
		void assignData(DbDataReader res);
		
	}
}