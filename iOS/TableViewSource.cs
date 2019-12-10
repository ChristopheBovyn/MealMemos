using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace MealMemos.iOS
{
    public class TableViewSource : UITableViewSource
    {
        public List<string> Dishes { get; set; }

        public Action<int> OnRemovedDish;
        public TableViewSource(List<string> dishes)
        {
            this.Dishes = dishes;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("customCell", indexPath); 
            cell.TextLabel.Text = Dishes[indexPath.Row];
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Dishes.Count;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    // delete the row from the table
                    //tableView.DeleteRows(indexPath, UITableViewRowAnimation.None);
                    this.OnRemovedDish?.Invoke(indexPath.Row);
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
    }
}