//
// XslIf.cs
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
	// also applicable to xsl:when
	internal class XslIf : XslCompiledElement {
		CompiledExpression test;
		XslOperation children;
		
		public XslIf (Compiler c) : base (c) {}
		
		protected override void Compile (Compiler c)
		{
			c.AssertAttribute ("test");
			c.Input.MoveToFirstAttribute ();
			do {
				if (c.Input.NamespaceURI != String.Empty)
					continue;
				switch (c.Input.LocalName) {
				case "test":
					test = c.CompileExpression (c.Input.Value);
					break;
				default:
					throw new XsltCompileException ("Invalid attribute was found: " + c.Input.Name, null, c.Input);
				}
			} while (c.Input.MoveToNextAttribute ());
			c.Input.MoveToParent ();

			if (!c.Input.MoveToFirstChild ()) return;
			children = c.CompileTemplateContent ();
			c.Input.MoveToParent ();
		}	
		
		public bool EvaluateIfTrue (XslTransformProcessor p)
		{
			if (p.EvaluateBoolean (test)) {
				if (children != null)
					children.Evaluate (p);
				return true;
			}
			return false;
		}

		public override void Evaluate (XslTransformProcessor p)
		{
			EvaluateIfTrue (p);
		}
	}
}
