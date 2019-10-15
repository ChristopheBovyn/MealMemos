using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace MealMemos.iOS
{
    public class TableViewSource : UITableViewSource
    {
        List<string> Items;
        string Identifier = "TableCell";

        public TableViewSource(List<string> items)
        {
            this.Items = items;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(Identifier);
            string item = Items[indexPath.Row];

            if(cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, Identifier);
                
            }
            cell.TextLabel.Text = item;
            cell.BackgroundColor = UIColor.Clear;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Items.Count;
        }

        public void addElement(string element)
        {
            this.Items.Add(element);
        }
    }
}
