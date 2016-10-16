
## Vehicle Service Station 
To design and realize software for management of a vehicle service station. Automobiles are repaired in the vehicle service station every day and repair cards, which contain information about the repairs performed, the input spare parts and labor are issued. There may be a multitude of repair cards for one automobile, and each of them corresponds to a repair performed. The system should provide various references related to the repairs performed.
 
#### Users
1. Users of the system are the operators who receive the customers in the vehicle service station and fill in information in the system. This information is shared amongst all the users but solely the user who created a certain repair card shall be entitled to edit it.
 
2. The following information is preserved for each user in the system:
 * username (it contains solely letters and digits, unique for the system)
 * password
 * name and surname
3. A special user with username "**admin**" and a preliminarily entered into the system password may add, edit and erase users. At erasure the users are deactivated, but remain in the system (aimed at not damaging the references).

4. Solely registered users (after login) and the administrator shall have access to the system.

#### Automobiles
1. The following information shall be maintained about each automobile:
 * State registration number (it should be obligatorily entered, for instance)
 * Brand
 * Model
 * Year of production
 * Number of frame – it should be obligatorily entered (it should be unique in the system)
 * Number of engine – it should be obligatorily entered
 * Color
 * Engine capacity (for instance, 500 cub. cm.)
 * Description (for other information)
 * Name of owner
 * Telephone for contact with the owner
2. The following operations shall be maintained for the automobiles (they may be performed by each operator):
 * Addition of an automobile– it is entered and the information about the automobile shall be stored.
 * Editing an automobile – it allows change of the information about a certain automobile

#### Spare parts
1. The spare parts have:
 * An identification number (a unique code in conformity with a catalogue)
 * Title (text name, for instance "a brake disc")
 * Price
2. Types of spare parts, which may be used at repairs of automobiles, are defined by the administrator. The thus already defined spare parts are used at the creation or editing a repair card.
3. The following operations (accessible solely for the administrator) shall be maintained for the spare parts:
 * Addition of a spare part.
 * Editing a spare part – it allows a change of the title or the price of a spare part already existing in the system. 
 * Erasure of a spare part – it erases an already defined spare part. The erasure makes the part inactive and thus it may be included in the reference but it is not accessible at new repairs. 

#### Employees (fitters)
The system should maintain a list with the employees in the vehicle service station (the fitters who perform the repairs). Each employee has a name and surname. The administrator may add / edit / erase fitters.
