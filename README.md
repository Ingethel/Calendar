# Calendar

This project was created for a naval museum as a scheduler in order to store their guided tours and have a good visualisation of the timetable to increase their productivity. 
It features three view modes; The monthly view is a compact version in order to have a full scale visualisation of the timeschedule without worrying about individual group details. 
It is the prefered version when in the process of booking a new group because the user can easily see information about the period around a certain date and conclude faster to an appropriate day/time.
The second most usefull view mode is the daily mode. There the user can add or review each group by looking at the full details.
The last view mode is the weekly, which gives full information of the user about each day of week and each appointment. 
It is not so visually appealing, but it is used in order to automatically generate the weekly report (exported in pdf).
Apart from the weekly report, the application also features the generation of a semester report which is more client specific.

Furthermore, there are a lot of setting for customisation and extensibility.
The timetable to each day of week can be altered in order to suit the customer needs.
Also the detailes that are stored for each group can be defined and the user can create a variety of such groups.
To add to that, colour groups can be defined, and when creating a new appointment you can set its special colour group to be visualised in the monthly view. 
That helps as a reminder to the user when there is a special appointment or occasion coming up.

As far as the data is concerned, they are stored in XML files. There is a file hierarchy as year>month>xmlfile and in the file they are separated by day as per parent>children. 
So when the application tries to retrieve information about a day, the algorithm goes directly to the associated year/month, so that it doesnt have to go through all the data.
