using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.Leanote.Synchronized;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 提交树
    /// </summary>
    public class SubmitTreeService
    {
        private DataContext dataContext;
        private IDistributedIdGenerator idGenerator;

        public SubmitTreeService(DataContext dataContext,
            IDistributedIdGenerator distributedIdGenerator
           )
        {
            this.dataContext = dataContext;
            this.idGenerator = distributedIdGenerator;
        }

        public void InitTree(long? Owner)
        {
            var tree = new SubmitTree()
            {
                Id = this.idGenerator.NextId(),
                Owner = Owner,
                Height = 0
            };
        }

        public SubmitTree GetSubmitTree(long id)
        {
            var tree = this.dataContext.SubmitTrees.Where(b => b.Id == id).First();
            return tree;
        }

        public SubmitBlock GetSubmitBlockByHeight(int height)
        {
            var block = this.dataContext.SubmitBlocks.Where(b => b.Height == height).First();
            return block;
        }

        public List<SubmitBlock> GetSubmitBlocksByTreeId(long treeId)
        {
            var blocks = this.dataContext.SubmitBlocks.Where(b => b.TreeId == treeId).ToList<SubmitBlock>();
            return blocks;
        }

        /// <summary>
        /// 增加提交块
        /// </summary>
        /// <param name="treeId"></param>
        /// <param name="submitBlock"></param>
        public void AddSubmitBlock(long? treeId, SubmitBlock submitBlock)
        {
            using var transaction = this.dataContext.Database.BeginTransaction();
            try
            {
                if (submitBlock.Id == null)
                {
                    submitBlock.Id = this.idGenerator.NextId();
                }
                var tree = this.dataContext.SubmitTrees.Where(b => b.Id == treeId).First();
                tree.Height++;
                if (tree.Height == 0)
                {
                    tree.Height = 1;
                    submitBlock.Height = 1;
                    submitBlock.TreeId = treeId;
                    submitBlock.PreBlockId = null;
                    this.dataContext.SubmitBlocks.Add(submitBlock);
                    tree.Top = submitBlock.Id;
                    tree.Root = submitBlock.Id;
                }
                else
                {
                    var preBlock = this.dataContext.SubmitBlocks.Where(b => b.Id == tree.Top);
                    submitBlock.Height = tree.Height;
                    submitBlock.TreeId = treeId;
                    submitBlock.PreBlockId = null;
                    this.dataContext.SubmitBlocks.Add(submitBlock);
                    tree.Top = submitBlock.Id;
                }
                this.dataContext.SubmitBlocks.Add(submitBlock);
                this.dataContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Dispose();
            }
        }

        public void AddSubmitOperation(long? repositoryId, SubmitOperation submitOperation, long? userId)
        {
            
            var tree=this.dataContext.SubmitTrees.Where(b=>b.Owner==repositoryId).First();
            var block = new SubmitBlock()
            {
                Id = this.idGenerator.NextId(),
                UserId = userId,
                TreeId = tree.Id,
                Date = DateTime.Now
            };
            submitOperation.SubmitBlockId = block.Id;
            this.dataContext.SubmitOperations.Add(submitOperation);
            AddSubmitBlock(tree.Id, block);
        }


     
    }
}