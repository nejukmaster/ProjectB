using ProjectB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ProjectB
{
    //아이템을 담는 Inventory 클래스
    public class Inventory : List<ItemStack>
    {
        public Action onInventoryUpdate;
        int size;

        public Inventory(int size) : base()
        {
            this.size = size;
        }

        public bool AddItem(ItemType type, int amount)
        {
            foreach (ItemStack stack in this)
            {
                    if (stack.GetType() == type)
                    {
                        stack.SetAmount(stack.GetAmount() + amount);
                        if(onInventoryUpdate != null)
                            onInventoryUpdate();
                        return true;
                    }
            }
            if(Count < size)
            {
                Add(new ItemStack(type, amount));
                if(onInventoryUpdate != null)
                    onInventoryUpdate();
                return true;
            }
            return false;
        }

        public bool RemoveItem(ItemType type, int amount)
        {
            for(int i = 0; i < this.Count; i++)
            {
                if (this[i].GetType() == type && this[i].GetAmount() >= amount)
                {
                    this[i].SetAmount(this[i].GetAmount() - amount);
                    if (this[i].GetAmount() == 0)
                    {
                        this.RemoveAt(i);
                    }
                    if(onInventoryUpdate != null)
                        onInventoryUpdate();
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItem(List<ItemStack> items)
        {
            if (this.Count <= 0) return false;

            bool flag = true;
            foreach(ItemStack stack in items)
            {
                foreach(ItemStack content in this)
                {
                    if (content.GetType() == stack.GetType() && content.GetAmount() <= stack.GetAmount())
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag) break;
            }
            if (flag)
            {
                foreach(ItemStack stack in items)
                {
                    RemoveItem(stack.GetType(), stack.GetAmount());
                }
                return true;
            }
            else return false;
        }

        public int GetSize()
        {
            return size;
        }
    }
}
