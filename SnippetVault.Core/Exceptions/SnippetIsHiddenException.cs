using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Exceptions
{
    public class SnippetIsHiddenException : Exception
    {
        public SnippetIsHiddenException() : base()
        {

        }

        public SnippetIsHiddenException(string? message) : base()
        {

        }

        public SnippetIsHiddenException(string? message, Exception? innerException) : base(message)
        {
            
        }
    }
}
