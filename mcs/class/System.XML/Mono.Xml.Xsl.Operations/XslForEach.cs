//
// XslForEach.cs
//
// Authors:
//	Ben Maurer (bmaurer@users.sourceforge.net)
//	Atsushi Enomoto (ginga@kit.hi-ho.ne.jp)
//	
// (C) 2003 Ben Maurer
// (C) 2003 Atsushi Enomoto
//

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
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations {
	internal class XslForEach : XslCompiledElement {
		XPathExpression select;
		XslOperation children;
		
		public XslForEach (Compiler c) : base (c) {}
		
		protected override void Compile (Compiler c)
		{
			c.AssertAttribute ("select");
			select = c.CompileExpression (c.GetAttribute ("select"));
			
			if (c.Input.MoveToFirstChild ()) {
				bool alldone = true;
				do {
					if (c.Input.NodeType == XPathNodeType.Text)
						{ alldone = false; break; }
					
					if (c.Input.NodeType != XPathNodeType.Element)
						continue;
					if (c.Input.NamespaceURI != Compiler.XsltNamespace)
						{ alldone = false; break; }
					if (c.Input.LocalName != "sort")
						{ alldone = false; break; }
						
					c.AddSort (select, new Sort (c));
					
				} while (c.Input.MoveToNext ());
				if (!alldone)
					children = c.CompileTemplateContent ();
				c.Input.MoveToParent ();
			}
		}
		
		public override void Evaluate (XslTransformProcessor p)
		{
			p.PushNodeset (p.Select (select));
			p.PushForEachContext ();
			
			while (p.NodesetMoveNext ())
				children.Evaluate (p);
			p.PopForEachContext();
			p.PopNodeset ();
		}
	}
}
