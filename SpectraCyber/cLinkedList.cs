using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCyber
{
    class cLinkedList
    {
        object mpListItem;
        cLinkedList mpNext;

        // Default Constructor
        public cLinkedList()
        {
            mpNext = null;
            mpListItem = null;
        }

        // Overwridden Constructor taking an object pointer
        public cLinkedList(object pObject)
        {
            mpNext = null;
            mpListItem = pObject;
        }

        ~cLinkedList()
        {
            if(mpNext != null)
                mpNext.Dispose(); // Destruct the next item

            //if(mpListItem != null)
            //    mpListItem.Dispose(); // Destruct the mpListItem

        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        // Append a cLinkedList item onto the list.
        public void Append(cLinkedList pList)
        {
            // If there's a mpNext, send the message down the list
            // If not, then mpNext becomes the input pList pointer
            if (mpNext != null)
                mpNext.Append(pList);
            else
                mpNext = pList;
        }

        // Append an object onto the list
        public void Append(object pObject)
        {
            cLinkedList pListItem = new cLinkedList(pObject);

            Append(pListItem);
        }

        // Remove the nth object from the list.
        public void RemoveAt(int iNumInList)
        {
            cLinkedList pPrev;
            cLinkedList pRemove;

            // Get the List object before the one we want
            pPrev = GetAt(iNumInList - 1);
            if (pPrev != null)
            {
                // Get the item list object we want
                pRemove = pPrev.mpNext;
                if (pRemove != null)
                {
                    // Make the preceding object 'point to' the object following the one to be removed
                    // Then make the object to be removed point to nothing, then delete it
                    pPrev.mpNext = pRemove.mpNext;
                    pRemove.mpNext = null;
                   // delete pRemove;   // Remove the pRemove Object
                }
            }
        }

        // Remove a specific object from the list
        public cLinkedList RemoveItem(object pItem)
        {
            cLinkedList pList;

            if (mpNext != null)
            {
                if (mpNext.mpListItem == mpListItem)
                {
                    pList = mpNext;
                    mpNext = pList.mpNext;
                    pList.mpNext = null;

                    return pList;
                }
                else
                    return mpNext.RemoveItem(pItem);
            }
            else
                return null;

        }

        // Get a reference to the nth cLinkedList object on the list.
        public cLinkedList GetAt(int iNumInList)
        {
            if (iNumInList == 0)
                return this;

            if (mpNext == null)
                return null;

            return mpNext.GetAt(iNumInList - 1);
        }

        // Get a reference to the nth object on the list.
        public object GetAtItem(int iNumInList)
        {
            return GetAt(iNumInList).mpListItem;
        }

        // Deteremine the length of the linked list.
        public int Length()
        {
            if (mpNext == null)
                return 0;

            return mpNext.Length() + 1;
        }
    }
}
