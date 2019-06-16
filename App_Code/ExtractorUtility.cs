using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Text;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace SocialSports.App_Code
{
    public class ExtractorUtility
    {
        public string ReadXmlByReader(string XmlFile)
        {
            XmlTextReader rdr = new XmlTextReader(XmlFile);
            StringBuilder s = new StringBuilder();

            while (rdr.Read())
            {
                switch (rdr.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        s.Append("Xml Declaration");
                        s.Append(rdr.Name);
                        s.Append(rdr.Value);
                        break;
                    case XmlNodeType.Element:
                        s.Append("Element");
                        s.Append(rdr.Name);
                        s.Append(rdr.Value);
                        break;
                    case XmlNodeType.Text:
                        s.Append(rdr.Value);
                        break;
                    default:
                        break;
                }

                if (rdr.AttributeCount > 0)
                {
                    while (rdr.MoveToNextAttribute())
                    {
                        s.Append("Attribute");
                        s.Append(rdr.Name);
                        s.Append(rdr.Value);
                    }

                }
            }

            rdr.Close();
            return s.ToString();
        }

        public string SelectNodeByName(string Xml, string StartElement, string NodeName, List<string> NodeChildrenNames)
        {
            XmlTextReader r = new XmlTextReader(Xml);
            StringBuilder s = new StringBuilder();

            r.ReadStartElement(StartElement);

            while (r.Read())
            {
                if (r.Name == NodeName && r.NodeType == XmlNodeType.Element)
                {
                    for (int i = 0; i < NodeChildrenNames.Count; i++)
                    {
                        r.ReadStartElement(NodeName);
                        s.Append(r.ReadElementString(NodeChildrenNames[i]));
                    }
                }
            }

            r.Close();
            return s.ToString();
        }

        public string GetChildNodes(XmlNodeList Nodes, int Level)
        {

          
                StringBuilder str = new StringBuilder();

                foreach (XmlNode Node in Nodes)
                {
                    switch (Node.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            str.Append(Node.Name);
                            str.Append(Node.Value);

                            break;
                        case XmlNodeType.Element:

                            str.Append(Node.Name);


                            break;
                        case XmlNodeType.Text:

                            str.Append(Node.Value);


                            break;
                        case XmlNodeType.Comment:

                            str.Append(Node.Value);


                            break;
                    }

                    if (Node.Attributes != null)
                    {
                        foreach (XmlAttribute Att in Node.Attributes)
                        {
                            str.Append(Att.Name);
                            str.Append(Att.Value);
                        }
                    }

                    if (Node.HasChildNodes)
                    {
                        str.Append(GetChildNodes(Node.ChildNodes, Level + 1));
                    }
                }

                return str.ToString();
            

        

        }

        public string GetXNav(XPathNavigator x, int level)
        {
            StringBuilder str = new StringBuilder();

            if (x.NodeType == XPathNodeType.Root)
            {
                str.Append(x.Name);
            }
            else if (x.NodeType == XPathNodeType.Element)
            {
                str.Append(x.Name); 
            }
            else if (x.NodeType == XPathNodeType.Text)
            {
                str.Append(x.Value); 

            }
            else if (x.NodeType == XPathNodeType.Comment)
            {
                str.Append(x.Value); 
            }

            x.MoveToFirstAttribute();

            do
            {

            } while (x.MoveToNextAttribute());

            x.MoveToParent();

            if (x.HasChildren)
            {
                x.MoveToFirstChild();
                do
                {
                    str.Append(GetXNav(x, level + 1));

                } while (x.MoveToNext());
            }

            return str.ToString();
        }


        public string ReadingWithXDocument(string xml)
        {
            XDocument doc = XDocument.Load(xml);

            StringBuilder s = new StringBuilder();
            foreach (XElement ele in doc.Element("example").Elements())
            {
                s.Append((string)ele.Element("Title"));
                s.Append((string)ele.Element("Director"));
            }

            return s.ToString();
        }

        public void SearchingWithXmlDocument(string xml, string tagName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            StringBuilder s = new StringBuilder();

            XmlNodeList lst = doc.GetElementsByTagName(tagName);

            foreach (XmlNode Node in lst)
            {
                s.Append(Node.ChildNodes[0].Value);
            }
        }

        public void SearchXml(string xml, string tagName, string searchText)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);

            StringBuilder s = new StringBuilder();

            XmlNodeList lst = doc.GetElementsByTagName(searchText);

            foreach (XmlNode n in lst)
            {
                string name = n.ChildNodes[0].Value;

                if (name == searchText)
                {
                    XmlNode Parent = n.ParentNode;

                    XmlNodeList child = ((XmlElement)Parent).GetElementsByTagName(tagName);

                    foreach (XmlNode node in child)
                    {
                        s.Append(node.ChildNodes[0].Value);
                    }
                }

            }
        }

        public string SelectXmlUsingXPAth(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xml);

            XmlNodeList nodes = document.SelectNodes("/DvdList/DVD/Title[../@Category='Science Fiction']");

            StringBuilder str = new StringBuilder();

            foreach (XmlNode node in nodes)
            {
                str.Append(node.ChildNodes[0].Value);
            }

            return str.ToString();
        }
        //Test This
        private List<string> ReadXml(string Url)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            List<string> StringsToRead = new List<string>();

            using (XmlReader rdr = XmlReader.Create(Url, settings))
            {
                if (rdr.NodeType == XmlNodeType.Element && rdr.LocalName == "resource")
                {
                    rdr.MoveToAttribute("classname");
                    rdr.MoveToElement();
                    rdr.ReadToDescendant("field");
                    StringsToRead.Add(rdr.ReadElementContentAsString());
                    rdr.MoveToElement();


                }



                return StringsToRead;
            }
        }

        private string GetPageHtmlDecoded(string Url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            req.UserAgent = "Johns Crawler";
            WebResponse res = req.GetResponse();
            Stream s = res.GetResponseStream();

            string PageData = string.Empty;

            using (StreamReader rdr = new StreamReader(s))
            {
                PageData += rdr.ReadToEnd();

            }
   
            return PageData;

        }

        public string[][] GetXmlNodesAndChildren(string Url, string NodeName)
        {
            string xml = GetPageHtmlDecoded(Url);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList NodeList = doc.SelectNodes(NodeName);
            string[][] ParentAndChild = new string[NodeList.Count][];

            int NodeCount = 0;
            foreach (XmlNode Node in NodeList)
            {
                if (Node.HasChildNodes)
                {
                    string[] ChildText = new string[Node.ChildNodes.Count];

                    int ChildNodeCount = 0;

                    foreach (XmlNode ChildNode in Node.ChildNodes)
                    {
                        if (ChildNode.InnerText != null)
                        {
                            ChildText[ChildNodeCount] = ChildNode.InnerText;
                        }
                    }

                    ParentAndChild[NodeCount] =  ChildText;

                }

                NodeCount++;
            }
            return ParentAndChild;

        }

        public static string[] GetImages(string[] ImageLinks)
        {
            List<string> ImageUrls = new List<string>();

            for(int i = 0; i < ImageLinks.Length; i++)
            {

                string ImageSuffix = ImageLinks[i].Substring(0, ImageLinks[i].Length - 3).ToLower();

                if(ImageSuffix == "jpg" || ImageSuffix == "png")
                {
                    WebClient Client = new WebClient();
                    byte[] ImageBytes = Client.DownloadData(ImageLinks[i]);

                    Stream fs =  new MemoryStream(ImageBytes);

                    using(BinaryReader rdr = new BinaryReader(fs))
                    {
                        byte[] bytesRead = rdr.ReadBytes((int)fs.Length);
                        string base64 = Convert.ToBase64String(bytesRead, 0, bytesRead.Length);
                    }
                }
            }

            return ImageUrls.ToArray();

        }
    }
}