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
    public class SubmitTreeService
    {
        private DataContext dataContext;
        private IDistributedIdGenerator idGenerator;

        public SubmitTreeService(DataContext dataContext,
            IDistributedIdGenerator distributedIdGenerator)
        {
            this.dataContext = dataContext;
            this.idGenerator = distributedIdGenerator;
        }

        public void InitTree(long Owner)
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
            var block=this.dataContext.SubmitBlocks.Where(b=>b.Height==height).First();
           return block;
        }
        public List<SubmitBlock> GetSubmitBlocksByTreeId(long treeId)
        {
            var blocks=this.dataContext.SubmitBlocks.Where(b=>b.TreeId==treeId).ToList<SubmitBlock>();
            return blocks;
        }



        public void AddBlock(SubmitBlock submitBlock)
        {
            this.dataContext.SubmitBlocks.Add(submitBlock);
        }



    }
}