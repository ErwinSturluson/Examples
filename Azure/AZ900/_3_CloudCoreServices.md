# **Azure Cloud Core Services**

|AZ-900 Domain Area|Weight|
|---|---|
|1. Describe cloud concepts|25-30%|
|2. Describe Azure architecture and services|35-40%|
|3. Describe Azure management and governance|30-35%|

## **Theory: Geo, Region, Zones, Availability Sets & Zones (Domain Area 2)**

**Availability Sets & Zones**

To make up Availablity Zone and Availability Sets definition you need to know what is Geo, Region and Zone.

**Geo (Geography)** - It's nothing but the physical boundary of a country, the physical map of the countries.

**Region** - It's an area inside Geo like States in The US or prefectures in Japan and in general regions have hundreds kilometers apart. If something happens in one region you will be able to shift your Data Center to another region is named **Paired Region** in hundreds kilometers away.

To see existing **Regions** and their **Paired Regions** follow the link below:

https://www.azurespeed.com/Information/AzureRegions

**Zones** - It's an area inside Region like cities, towns and villages where actual Data Centers placed are in physical buildings.

In Azure you be able to choose only a zone but region is choosen automatically. When something bad happens in this a **Region**, a Data Center will be move to related **Paired Region**.

**Availability Zones** - Availability zone (AZ) and Availability Set (AS) are about when one machine goes down gow quckly we will recover it as another machine in another zone across different data centers. When one zone or Data Center building goes down hwo quckly we will be able to switch to another zone. It's all about achieving SLA about 99.99% uptime.

**Availability Sets** - It's about when inside a zone if something went wrong witch your rack and it went down you can make a switch to another rack or another machine either in the same data center or even in the same building 

**Paierd Region** can be choosen only automatically but **Zones** and **Sets** must be choosen manually.

## **Question 12: Data Center & Availability Zones (Domain Area 2)**

If you need to ensure the services running on the VMs are available if a single Data Center fails, you need to deploy the virtual machines to two or mopre availability zones.

## **Question 13: Do all regions support zones (Domain Area 2)**

If you have Azure resources deployed to every region you cannot implement eveilability zones in all the regions because not all regions support availability zones e.g. Middle East doesn't support availability zones.

Availability zones can be used for every kind of resources, not only for VM's.

Availability zones aren't used to replicate data and applications to multiple regions because Availability zones is about actions inside a Region but not across a Region inside a Geo.

## **Question 14: Can scale sets help when data center fails ?**

If you have many Azure virtual machines and you need to ensure that the services running on the machines are available a single Data Center fails and you put virtual machines to two or more scale sets it isn't enough becouse you need to specify availability zone so if you don't specify Availability zone option it's don't guarantee a VM will be created in the other Data Center.

**Scale Set** - is a set of VM's provides High Availability when some of VM's goes down it creates new VM's and let consumers use it. If one VM goes down an another VM will come into action.






