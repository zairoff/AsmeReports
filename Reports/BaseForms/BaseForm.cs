using System.Drawing;
using System.Windows.Forms;

namespace Reports.BaseForms
{
    public class BaseForm : Form
    {
        public void FindByText(TreeNode tree, TreeView treeView)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            if (nodes.Count < 1)
            {
                treeView.Nodes.Add(tree);
                return;
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name == tree.Name.Replace("." + tree.Text.Replace(" ", ""), ""))
                {
                    nodes[i].Nodes.Add(tree);
                    return;
                }
                addNodes(nodes[i], tree);
            }
        }

        private void addNodes(TreeNode treeNode, TreeNode tree)
        {
            for (int i = 0; i < treeNode.Nodes.Count; i++)
            {
                if (treeNode.Nodes[i].Name == tree.Name.Replace("." + tree.Text.Replace(" ", ""), ""))
                    treeNode.Nodes[i].Nodes.Add(tree);
                addNodes(treeNode.Nodes[i], tree);
            }
        }

        public void ClearBackColor(TreeView treeView)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                ClearRecursive(nodes[i], treeView);
            }
        }

        private void ClearRecursive(TreeNode treeNode, TreeView treeView)
        {
            treeView.Nodes[0].BackColor = Color.White;
            treeView.Nodes[0].ForeColor = Color.Black;
            for (int i = 0; i < treeNode.Nodes.Count; i++)
            {
                treeNode.Nodes[i].BackColor = Color.White;
                treeNode.Nodes[i].ForeColor = Color.Black;
                ClearRecursive(treeNode.Nodes[i], treeView);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.ClientSize = new System.Drawing.Size(412, 388);
            this.Name = "BaseForm";
            this.ResumeLayout(false);

        }
    }
}
