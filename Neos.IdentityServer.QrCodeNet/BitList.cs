﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2011 George Mamaladze                                                                                                                                                      //
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Neos.IdentityServer.MultiFactor.QrEncoding
{
	internal sealed class BitList : IEnumerable<bool>
	{
		private readonly List<byte> m_Array;
		private int m_BitsSize;
		
		internal BitList()
		{
			m_BitsSize = 0;
			m_Array = new List<byte>(32);
		}

        internal BitList(IEnumerable<byte> byteArray)
        {
            m_BitsSize = byteArray.Count();
            m_Array = byteArray.ToList();
        }
		
		internal List<byte> List
		{
			get
			{
				return m_Array;
			}
		}
		
		public IEnumerator<bool> GetEnumerator()
		{
			int numBytes = m_BitsSize >> 3;
			int remainder = m_BitsSize & 0x7;
			byte value;
			for(int index = 0; index < numBytes; index++)
			{
				value = m_Array[index];
				for(int shiftNum = 7; shiftNum >= 0; shiftNum--)
				{
					yield return ((value >> shiftNum) & 1) == 1;
				}
			}
			if(remainder > 0)
			{
				value = m_Array[numBytes];
				for(int index = 0; index < remainder; index++)
				{
					yield return ((value >> (7 - index)) & 1) == 1;
				}
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }
		
		internal bool this[int index]
		{
			get
			{
				if(index < 0 || index >= m_BitsSize)
					throw new ArgumentOutOfRangeException("index", "Index out of range");
				int value_Renamed = m_Array[index >> 3] & 0xff;
				return ((value_Renamed >> (7 - (index & 0x7))) & 1) == 1;
			}
		}
		
		private int ToBit(bool item)
		{
			return item ? 1 : 0;
		}
		
		internal void Add(bool item)
		{
			int numBitsinLastByte = m_BitsSize & 0x7;
			//Add one more byte to List when we have no bits in the last byte. 
			if(numBitsinLastByte == 0)
				m_Array.Add(0);
			
			m_Array[m_BitsSize >> 3] |= (byte)(ToBit(item) << ( 7 - numBitsinLastByte));
			m_BitsSize++;
		}
		
		internal void Add(IEnumerable<bool> items)
		{
			foreach(bool item in items)
			{
				this.Add(item);
			}
		}
		
		internal void Add(int value, int bitCount)
		{
			if(bitCount < 0 || bitCount > 32)
				throw new ArgumentOutOfRangeException("bitCount", "bitCount must greater or equal to 0");
			int numBitsLeft = bitCount;
			
			while(numBitsLeft > 0)
			{
				if((m_BitsSize & 0x7) == 0 && numBitsLeft >= 8)
				{
					//Add one more byte to List. 
					byte newByte = (byte)((value >> (numBitsLeft - 8)) & 0xFF);
					this.appendByte(newByte);
					numBitsLeft -= 8;
				}
				else
				{
					bool bit = ((value >> (numBitsLeft - 1)) & 1) == 1;
					this.Add(bit);
					numBitsLeft--;
				}
			}
		}
			
		private void appendByte(byte item)
		{
			m_Array.Add(item);
			m_BitsSize += 8;
		}
		
		internal int Count
		{
			get
			{
				return m_BitsSize;
			}
		}
		
		internal int SizeInByte
		{
			get
			{
				return (m_BitsSize + 7) >> 3;
			}
		}
		
	}
}
