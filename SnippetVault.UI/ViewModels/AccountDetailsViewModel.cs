using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.ApplicationUserDTOs;
using SnippetVault.Core.Validators;
using System.ComponentModel.DataAnnotations;

namespace SnippetVault.UI.ViewModels
{
    public class AccountDetailsViewModel
    {
        public UpdateAccountDTO? UpdateAccountDTO { get; set; }

        public int SnippetsCount { get; set; }
        public int StarsCount { get; set; }
        public int CommentsCount { get; set; }
        public int CommentLikesCount { get; set; }
    }
}