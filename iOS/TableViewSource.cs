﻿using System;
using System.Collections.Generic;
using Foundation;
using Plugin.CloudFirestore;
using UIKit;


namespace MealMemos.iOS
{
    public class TableViewSource : UITableViewSource
    {
        public string Identifier = string.Empty;
        List<string> Items;
        public TableViewSource(List<string> items)
        {
            this.Items = items;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (CustomViewCellController)tableView.DequeueReusableCell(CustomViewCellController.Identifier, indexPath);
            string item = Items[indexPath.Row];
            cell.Configure(item);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Items.Count;
        }

        public void AddElement(string element)
        {
            this.Items.Add(element);
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // remove the item from the underlying data source
                    this.Items.RemoveAt(indexPath.Row);
                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.None);
                    this.Save();
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true; // return false if you wish to disable editing for a specific indexPath or for all rows
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {   // Optional - default text is 'Delete'
            return "Delete";
        }

        public void Save()
        {
            var map = new Dictionary<string, Object>();
            for (int i = 0; i < this.Items.Count; i++)
            {
                map.Add("dish" + (i + 1), this.Items[i]);
            }
            var doc = CrossCloudFirestore.Current.Instance.GetCollection("meals").GetDocument(this.Identifier);
            try
            {
                if(map.Count == 0)
                {
                    doc.DeleteDocumentAsync();
                }else
                {
                    doc.SetDataAsync(map);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}