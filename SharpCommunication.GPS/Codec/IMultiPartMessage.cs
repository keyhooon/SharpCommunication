﻿//  *******************************************************************************
//  *  Licensed under the Apache License, Version 2.0 (the "License");
//  *  you may not use this file except in compliance with the License.
//  *  You may obtain a copy of the License at
//  *
//  *  http://www.apache.org/licenses/LICENSE-2.0
//  *
//  *   Unless required by applicable law or agreed to in writing, software
//  *   distributed under the License is distributed on an "AS IS" BASIS,
//  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  *   See the License for the specific language governing permissions and
//  *   limitations under the License.
//  ******************************************************************************

using System.Collections.Generic;

namespace SharpCommunication.Codec
{
    internal interface IMultiPartMessage : System.Collections.IEnumerable
	{
		/// <summary>
		/// Total number of messages of this type in this cycle
		/// </summary>
		int TotalMessages { get; }

		/// <summary>
		/// Message number
		/// </summary>
		int MessageNumber { get; }
	}

    internal interface IMultiPartMessage<T> : IMultiPartMessage, IEnumerable<T>
    {
	}
}
