using System;
namespace General {
	public class MyDate {
		public int day;
		public int month;
		public int year;

		public MyDate(string date) {


			this.day = ((Convert.ToInt32((Convert.ToString(date[0])))) * 10) + (Convert.ToInt32((Convert.ToString(date[1]))));
			this.month = ((Convert.ToInt32((Convert.ToString(date[3])))) * 10) + (Convert.ToInt32((Convert.ToString(date[4]))));
			this.year = ((Convert.ToInt32((Convert.ToString(date[8])))) * 10) + (Convert.ToInt32((Convert.ToString(date[9])))); ;

		}

	}
}
