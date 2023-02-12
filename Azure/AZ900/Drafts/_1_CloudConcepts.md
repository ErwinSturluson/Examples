# **Azure Cloud Concepts**

## **Chapter 01: What is AZ-900 Certification** 

**AZ-900 Certification** - is an certificate is given for successfully passed **AZ-900 Exam** checks foundational knowledge of **Cloud Services** and how those services are provided with **Microsoft Azure**. 

Exam page: https://learn.microsoft.com/en-us/certifications/exams/az-900/

Learning page: https://learn.microsoft.com/en-us/training/modules/describe-cloud-compute/1-introduction-microsoft-azure-fundamentals

## **Chapter 02: Syllabus and Intention of the Exam** 

Syllabus includes three knowledge domain areas:

|AZ-900 Domain Area|Weight|
|---|---|
|1. Describe cloud concepts|25-30%|
|2. Describe Azure architecture and services|35-40%|
|3. Describe Azure management and governance|30-35%|


## **Chapter 03: How is the course is structured**

**A topic:**

Concept -> Question -> Answer

**Explanation of a topic:**

Question -> Concept -> Answer

## **Question 1: Why Data centre is Capex and Azure VMs Opex ? (Domain Area 1)**
---

### **CapEx & OpEx**

**CapEx (Capital Expenditure)** - expenditure on hardware configuration such as RAM, CPU and hard disk, 
licensed software such as OS, MS Office, antivirus or others. 
It's upfront hardware and software cost which you need to invest right now.

**OpEx (Operational Expenditure)** - expenditure on running costs also attached to Capex like salary of employee who looks after the hardware and software or additional media drive to store backups.

If you are going to go and build your infrastructure manually, you have these two kind of cost: **CapEx** and **OpEx**;

**Pay-As-You-Go** - it is a rental model when you talk about the cloud. It's opposite to CapEx and isn't example of CapEx.

**OpEx** - for example, when you use Azure VM instances for few days and delete it.

**CapEx** - for example, when you created your own data center and you are responsible for its hardware, 
licenses of software and need to look after the infrastructure and also you are responsible for **OpEx**.


## **Question 2: B2S Burst VMs cost & VM Storage cost (Domain Area 1)**

Azure provides flexibility between CapEx and OpEx.

B2s size VM is a Burstable VM Size is when monthly cost depends on a VM usage e.g. in general you use 40% of 
VM size and sometimes VM's usage shoot up to 200% usage and you pay for 200% usage only for this high-load time.

It's possible to get different costs for the B2S size virtual machines depends on it's usage.

When you take a virtual machine or any cloud drive, you pay for 2 things:
- To just keep the virtual machine running - baseline cost;
- Usage cost: when you are using more than 100% of CPU.
and you will get billed for this period as well as definitely for the usage.

When the baseline cost will be reduced

If an Azure VM is stopped you continue to pay storage costs associated to the VM e.g. because even VM was stopped 
it's still stores data.

## **Question 3: Is VM IAAS , PAAS or SAAS ? (Domain Area 1)**

when you bring anything out there on cloud, you are buying resource from one of these categories and the product belongs to one of these categories:

**IaaS (Infrastructure as a Service)** - It means you buy some hardware to use e.g. VM with 2 Cores CPU, 8GB RAM and 20GB Hard Disk.

**PaaS (Platform as a Service)** - E.g. Cloud Storage, Cloud Fabric, App Service, Database Service etc. and other platform services which includes software are tightly coupled to your framework and they are available at a very cheap rate.

**SaaS (Software as a Service)** - E.g. MS SQL Server which even no need to install manually to either Windows or Linux machine and it's available in a rental model. So rather than buying the while sequence of a license in one go, now you can use the monthly rental model. Also it's software marketplaces.

**FaaS (Function as a Service)** - It means you can host just a simple function and pay as you go depends upon this function usage of hardware resources.

## **Question 4: Why SQL Server installed in VM not PAAS ? (Domain Area 1)**

Because rent a VM is already IaaS. You need to choose MS SQL Server Database Service instead.

## **Question 5: How Administrative effort is reduced with PAAS? (Domain Area 1)**

By using Platform as a Service.

## **Question 6: Does PAAS control on operating system? (Domain Area 1)**

No, you don't have any kind of access to OS using PAAS.

But a PaaS solution povides the ability to scale the platform automatically.

A PaaS solution also provides professional development services to continiously add features to custom applications (via CI/CD).

## **Question 7: Do we need to configure high availability in SAAS mode ? (Domain Area 1)**

No, but we need to configure the SaaS solution so setup parameters, business rules and so on.

For example: we can setup a Web Application from marketplace as a SaaS and just define its scalability rules to start.