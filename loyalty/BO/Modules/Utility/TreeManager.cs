using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palmary.Loyalty.BO.DataTransferObjects.Tree;


namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class TreeManager
    {
        public static IList<CategoryNode> BuildTree(IEnumerable<CategoryNode> source)
        {
            var roots = new List<CategoryNode>();

            if (source.Count() > 0)
            {
                var groups = source.GroupBy(i => i.ParentID);

                roots = groups.FirstOrDefault(g => g.Key == 0).ToList(); // get root nodes

                if (roots.Count > 0)
                {
                    var dict = groups.Where(g => g.Key != 0).ToDictionary(g => g.Key, g => g.ToList());
                    for (int i = 0; i < roots.Count; i++)
                    {
                        AddChildren(roots[i], dict);
                    }
                }
            }
            return roots;
        }

        private static void AddChildren(CategoryNode node, IDictionary<int, List<CategoryNode>> source)
        {
            if (source.ContainsKey(node.id))
            {      
                node.children = source[node.id];
                for (int i = 0; i < node.children.Count; i++)
                    AddChildren(node.children[i], source);
            }
            else
            {
                node.children = new List<CategoryNode>();
            }
        }

        public static IList<CategoryNode> BuildTree_selectList(IEnumerable<CategoryNode> source)
        {
            var nodes = BuildTree(source);

            var renamedList = new List<CategoryNode>();

            foreach (var n in nodes)
            {
                renamedList.Add(n);
                if (n.children.Count() > 0)
                {
                    SelectList_changeName(n, n.children, renamedList);
                }
            }
            return renamedList;
        }

        private static void SelectList_changeName(CategoryNode parentNode, List<CategoryNode> childrenList, List<CategoryNode> renamedList)
        {
            foreach (var n in childrenList)
            {
                n.text = parentNode.text + "/" + n.text;
                renamedList.Add(n);

                if (n.children.Count() > 0)
                {
                    SelectList_changeName(n, n.children, renamedList);
                }
            }
        }
    }
}
