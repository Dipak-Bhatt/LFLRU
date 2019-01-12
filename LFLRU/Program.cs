using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThesisLastUpdate
{
    //this for to show page status
    public enum Pagestatus { dirty, clean };

    //node structure
    class LFLRUnode
    {

        public LFLRUnode prev { get; set; }
        public object Data { get; set; }
        public LFLRUnode next { get; set; }
        public Pagestatus status;

        public LFLRUnode()
        {
            Data = null;
            prev = null;
            next = null;
        }

        public LFLRUnode(object data, Pagestatus Pstatus)
        {

            status = Pstatus;
            Data = data;
            prev = null;
            next = null;
        }

        // returns entire list
        public LFLRUnode(Pagestatus statuss, LFLRUnode datavalue)
            : this(statuss, null, datavalue, null)
        {
        }

        //this constructor fills all parameters on their respective fields
        public LFLRUnode(Pagestatus statuss, LFLRUnode previousnode, LFLRUnode datavalue, LFLRUnode nextnode)
        {
            status = statuss;
            prev = previousnode;
            Data = datavalue;
            next = nextnode;
        }
    }


    /// <summary>
    /// Class for List of nodes with all necessary operation in the list 
    /// </summary>
    class cache
    {
        public LFLRUnode firstnode { get; set; }
        public LFLRUnode lastnode { get; set; }
        public string name;

        /// <summary>
        /// Initialization of the list
        /// </summary>
        /// <param name="listname"></param>
        public cache(string listname)
        {
            name = listname;
            firstnode = null;
            lastnode = null;
        }
        public cache()
            : this("list")
        { }

        /// <summary>
        /// Insert node at the firts position of the list
        /// </summary>
        /// <param name="insertNode"></param>
        public void InsertAtFront(LFLRUnode insertNode)
        {
            if (IsEmpty())
            {
                firstnode = lastnode = insertNode;
            }
            else
            {
                insertNode.next = firstnode;
                firstnode.prev = insertNode;
                firstnode = insertNode;
            }

        }

        /// <summary>
        /// To check whether the list is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return firstnode == null;
        }


        /// <summary>
        /// To search the node in the list
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public LFLRUnode findnode(LFLRUnode node)
        {
            LFLRUnode newnode = new LFLRUnode();

            for (LFLRUnode temp = firstnode; temp != lastnode.next; temp = temp.next)
            {
                if (temp.Data.Equals(node.Data))
                {
                    newnode = temp;
                }
            }
            return newnode;
        }

        /// <summary>
        /// Search the node in the List
        /// </summary>
        /// <param name="nodetobeSearched"></param>
        /// <returns></returns>
        public bool search(LFLRUnode nodetobeSearched)
        {
            bool checkpoint = false;

            if (IsEmpty())
            {
                checkpoint = false;
            }

            else
            {
                for (LFLRUnode temp = firstnode; temp != lastnode.next; temp = temp.next)
                {
                    if (temp.Data.Equals(nodetobeSearched.Data))
                    {
                        checkpoint = true;
                    }
                }
            }
            return checkpoint;
        }

        public bool CheckCleanPage(int noOfStepsTomove)
        {
            bool checkpoint = false;
            int count = 0;
            if (IsEmpty())
            {
                checkpoint = false;
            }

            else
            {
                while (noOfStepsTomove > count)
                {
                    for (LFLRUnode temp = lastnode; temp != firstnode.next; temp = temp.prev)
                    {
                        if (temp.status.Equals(Pagestatus.clean))
                        {
                            //Remove node from here
                            checkpoint = true;
                        }
                    }
                    count++;
                }
            }
            return checkpoint;
        }


        //get least recent used block
        public LFLRUnode GetLeastUsedCleanNode(int length)
        {
            LFLRUnode newnode = new LFLRUnode();
            var count = 0;
            for (LFLRUnode temp = lastnode; temp != firstnode.prev; temp = temp.prev)
            {
                if (length > count)
                {
                    if (temp.status.Equals(Pagestatus.clean))
                    {
                        newnode = temp;
                    }
                }
                else
                    break;
            }
            return newnode;
        }

        /// <summary>
        /// Moving the Current page to the first position of the list
        /// </summary>
        /// <param name="node"></param>
        public void MoveAtFirsPos(LFLRUnode node)
        {
            if (node.prev == null)
            {
                //Do nothing since the node itself is first node
            }

            else if (node.next == null)
            {
                node.prev.next = null;
                lastnode = node.prev;
                node.next = firstnode;
                node.prev = null;
                firstnode.prev = node;
                firstnode = node;

            }
            else
            {
                node.prev.next = node.next;
                node.next.prev = node.prev;
                node.next = firstnode;
                node.prev = null;
                firstnode.prev = node;
                firstnode = node;
            }
        }


        /// <summary>
        /// To delete the node tempom the list
        /// </summary>
        /// <param name="delenode"></param>
        public void Deletenode(LFLRUnode delenode)
        {
            for (LFLRUnode temp = firstnode; temp != lastnode.next; temp = temp.next)
            {
                if (temp.Data.Equals(delenode.Data))
                {
                    if (temp.next == null)
                    {
                        lastnode = lastnode.prev;
                        lastnode.next = null;
                        //temp.prev = null;
                    }
                    else if (temp.prev == null)
                    {
                        firstnode = firstnode.next;
                        firstnode.prev = null;
                        //temp.next = null;
                    }
                    else
                    {
                        temp.next.prev = temp.prev;
                        temp.prev.next = temp.next;
                        //temp.prev = null;
                        //temp.next = null;
                    }
                }
            }
        }

        public void RemoveLastNode()
        {
            lastnode = lastnode.prev;
            lastnode.next = null;
        }

        /// <summary>
        /// To show the Current status of the all nodes in the list
        /// </summary>
        public void showstatus()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Empty " + name);
            }

            else
            {
                Console.WriteLine("\t");
            }
        }
    }
    //Main class starts from here
    class LFLRU : LFLRUnode
    {
        public static void Main()
        {
            double mediumValue; //var will determine the range of MRU and LRU.
            int totalsize;//hold the user inputed cache size
            int np = 0, pageCount = 0, pagehit = 0, pagefault = 0, distinctpages = 0;
            Console.WriteLine("Algorithm: LFLRU");
            Console.WriteLine("Cache size:");
            totalsize = int.Parse(Console.ReadLine());//reads user inputed cache size
            mediumValue = totalsize / 2;
            //check whole and double number, if it is whole number then do nothing otherwise it makes whole number with ceiling function.
            if (Math.Floor(mediumValue) != mediumValue)
            {
                mediumValue = Math.Ceiling(mediumValue);
            }
            LFLRUnode node;// temporary node

            using (StreamReader r = new StreamReader("D:\\sprite.trc")) // give file directory here, reads file based on this directory
            {
                cache newlist = new cache();
                string refpage;

                while ((refpage = r.ReadLine()) != null) // reads entire lines included within file
                {
                    var distinguishedpageNPageStatus = refpage.Split(',');//splits input string by , and convert this into array
                    object data = distinguishedpageNPageStatus[1];

                    if (distinguishedpageNPageStatus[0].Trim() == "0")
                        node = new LFLRUnode(data, Pagestatus.clean); // creates new instance 
                    else
                        node = new LFLRUnode(data, Pagestatus.dirty);
                    pageCount++;
                    //if page is find in the list
                    if (newlist.search(node))
                    {
                        //Find that particular node in the list
                        LFLRUnode CurrentPage = newlist.findnode(node);
                        //Move  Current page at the begining of the list
                        newlist.MoveAtFirsPos(CurrentPage);
                    }
                    //page in not in the list
                    else
                    {
                        np++;
                        //admit the newly acccessed block to the MRU list till there is no more free slot
                        if (np <= totalsize)
                        {
                            newlist.InsertAtFront(node);
                            //    newlist.showstatus();
                        }
                        //promote the newly accessed 
                        else
                        {
                            int noOfStepsTomove = totalsize - Convert.ToInt32(mediumValue);
                            if (newlist.CheckCleanPage(noOfStepsTomove))
                            {
                                //get least used clean page
                                var cleanBlock = newlist.GetLeastUsedCleanNode(noOfStepsTomove);
                                //Remove clean node 
                                newlist.Deletenode(cleanBlock);
                                //Add new at the front
                                newlist.InsertAtFront(node);
                            }
                            else
                            {
                                //Remove Last node from list
                                newlist.RemoveLastNode();
                                //Add new one at the front
                                newlist.InsertAtFront(node);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Total Number of Pages:" + pageCount);
            Console.WriteLine("Total Number of distinct Pages:" + distinctpages);
            Console.WriteLine("Totol Number Of Page fault:" + pagefault);
            Console.WriteLine("Totol Number Of Page Hit:" + pagehit);
            Console.ReadLine();


        }
    }

}
