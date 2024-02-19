namespace SnippetVault.Core.Domain.IdentityEntities
{
    // Moderator doesn't have snippets and comments, it is need to hide comments or snippets if inappropriate
    // content is created, because of this it is different user not different role
    public class ModeratorUser: BaseUser
    {
    }
}