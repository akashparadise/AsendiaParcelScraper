# AsendiaParcelScraper
Assignment for the role of IT Senior Software Developer(.NET) with Asendia

Instructions
The purpose of the exercise is to evaluate your approach to software development including object-oriented design, design patterns and testing.
•	Complete the exercise in C#
•	Structure your code as if this were a real production application
•	State any assumptions you make as comments in the code. If any aspect of the specification is unclear, state your interpretation of the requirement in a comment.

Task
Write a c# console application that monitors a directory for csv files, reads them in and outputs a file in XML format.
The CSV to be imported has 14 columns and a header (example attached: CandidateTest_ManifestExample.csv). The 14th column is optional. If it is blank then the value should default to GBP.
Each line in the CSV file (aside from the header) represents an item in a parcel. An item belongs to a parcel if it matches the Parcel Code. Parcels belong to a Consignment if the Consignment No matches. Consignments belong to an Order if the Order No matches.
The XML file output should have the following nodes:
•	Orders
•	Order
•	Consignments
•	Consignment
•	Parcels
•	Parcel
•	Parcel Items
•	Parcel Item
Additionally within the Orders node there should be a Total Value and Total Weight node representing the total value and total weight of all the consignments for that Order. 
Deliverables
A .Net project written in C# that meets the requirements of the task. Please include all files required to build and run the software.
