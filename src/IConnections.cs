/**
 * This file is part of the Elberos package.
 *
 * (c) Ildar Bikmamatov <elberos@bayrell.org>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using Microsoft.Extensions.Configuration;

namespace Elberos.Orm{
	
	public interface IConnections {
		
		void add(string api_name, Connection conn);
		void remove(string api_name);
		Connection get(string api_name);
		
		void Configure(IConfiguration config);
	}
}