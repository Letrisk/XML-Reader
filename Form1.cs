using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        XmlDocument xDoc = new XmlDocument();

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "*xml files (*.xml)|*.xml"; //фильтр для открытия и сохранения только xml
            saveFileDialog1.Filter = "*xml files (*.xml)|*.xml";
        }

        //метод добавляет узел xml файла в соответствующий узел дерева
        private static void addNode(XmlNode selectedXNode, TreeNode selectedTreeNode, int level = 0)
        {
            foreach(var child in selectedXNode.ChildNodes) //перебор всех дочерних элементов в узле xml файла
                                                           //используется var, т.к. неизвестно что за тип элемента
            {
                //если текущий элемент узел, то добавляем его в дерево и рекурсивно вызываем этот метод
                if (child is XmlElement node)
                {
                    selectedTreeNode.Nodes.Add(getNameWithAttributes(node));
                    addNode(node, selectedTreeNode.Nodes[level]);
                    level++;
                }
                //если элемент текст, то просто добавляем в дерево
                if (child is XmlText text)
                {
                    selectedTreeNode.Nodes.Add($"-{text.InnerText}");
                }
            }
        }

        //функция выдает строку с именем и аттрибутами элемента
        private static string getNameWithAttributes(XmlElement element)
        {
            string nameAttr = element.Name;
            foreach(XmlAttribute attr in element.Attributes)
                nameAttr += $"[{attr.Name}={attr.InnerText}]";
            return nameAttr;
        }


        //открытие файла и первый запуск метода addNode
        private void openButton_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();//очистка перед каждым новым открытием

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xDoc.Load(openFileDialog1.OpenFile());
                XmlElement xRoot = xDoc.DocumentElement;
                XmlNode xNode = xDoc.ChildNodes[1];

                treeView1.Nodes.Add(xRoot.Name);

                TreeNode tNode = treeView1.Nodes[0];
                addNode(xNode, tNode);
                return;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xDoc.Save(saveFileDialog1.FileName);
                return;
            }
        }
    }
}
