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
using Microsoft.Extensions.Configuration;

namespace Elberos.Orm{
	
	public class Connections: IConnections {
		
		protected Dictionary<string, Connection> _connections = null;
		
		public Connections(){
			this._connections = new Dictionary<string, Connection>();
		}
		public void add(string api_name, Connection conn){
			this._connections.Add(api_name, conn);
		}
		public void remove(string api_name){
			this._connections.Remove(api_name);
		}
		public Connection get(string api_name){
			if (api_name == null)
				api_name = "default";
			
			if (!this._connections.ContainsKey(api_name))
				return null;
			return this._connections[api_name];
		}
		
		
		public void Configure(IConfigurationSection config){
			
			foreach (IConfigurationSection section in config.GetChildren()){
				string name = section["name"];
				string type = section["type"];
				Type the_type = Type.GetType(type);
				
				Connection connection = (Connection)Activator.CreateInstance(the_type);
				connection.Configure(section);
				this.add(name, connection);
			}
			
		}
	}
}