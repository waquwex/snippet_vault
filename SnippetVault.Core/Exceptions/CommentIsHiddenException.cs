using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Exceptions
{
    public class CommentIsHiddenException : Exception
    {
        public CommentIsHiddenException() : base()
        {

        }

        public CommentIsHiddenException(string? message) : base()
        {

        }

        public CommentIsHiddenException(string? message, Exception? innerException) : base(message)
        {

        }
    }
}
