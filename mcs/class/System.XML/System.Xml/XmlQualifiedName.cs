//
// System.Xml.XmlQualifiedName.cs
//
// Author: Duncan Mak (duncan@ximian.com)
//		 
// (C) Ximian, Inc.
// 
// Modified: 
//		21st June 2002 : Ajay kumar Dwivedi (adwiv@yahoo.com)

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace System.Xml
{
	public class XmlQualifiedName
	{
		// Constructors		
		public XmlQualifiedName ()
			: this (string.Empty, string.Empty)
		{
		}

		public XmlQualifiedName (string name)
			: this (name, string.Empty)
		{
		}

		public XmlQualifiedName (string name, string ns)
			: base ()
		{
			this.name = (name == null) ? "" : name;
			this.ns = (ns == null) ? "" : ns;
			this.hash = this.name.GetHashCode () ^ this.ns.GetHashCode ();
		}

		// Fields
		public static readonly XmlQualifiedName Empty = new XmlQualifiedName ();		
		private readonly string name;
		private readonly string ns;
		private readonly int hash;
		
		// Properties
		public bool IsEmpty
		{
			get {
				return name.Length == 0 && ns.Length == 0;
			}
		}

		public string Name
		{
			get { return name; }
		}

		public string Namespace
		{
			get { return ns; }
		}

		// Methods
		public override bool Equals (object other)
		{
			return this == (other as XmlQualifiedName);
		}

		public override int GetHashCode () 
		{ 
			return hash;
		}

		public override string ToString ()
		{
			if (ns == string.Empty)
				return name;
			else
			 	return ns + ":" + name;
		}

		public static string ToString (string name, string ns)
		{
			if (ns == string.Empty)
				return name;
			else				
				return ns + ":" + name;
		}

		// Operators
		public static bool operator == (XmlQualifiedName a, XmlQualifiedName b)
		{
			if((Object)a == (Object)b)
				return true;

			if((Object)a == null || (Object)b == null)
				return false;

			return (a.hash == b.hash) && (a.name == b.name) && (a.ns == b.ns);
		}

		public static bool operator != (XmlQualifiedName a, XmlQualifiedName b)
		{
			return !(a == b);
		}
	}
}
