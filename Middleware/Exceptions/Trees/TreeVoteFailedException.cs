using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrzewaAPI.Middleware.Exceptions;

public class TreeVoteFailedException : BusinessException
{
    public TreeVoteFailedException(Guid treeId, string reason) : base($"Nie udało się zagłosować na drzewo {treeId}: {reason}", "TREE_VOTE_FAILED", 400)
    {
    }
}
