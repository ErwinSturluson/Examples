**Cloud Computing** is a delivery model for services like storage and compute power resources.

**Cloud Services** is a services are delivered over the internet.

---
## **Scaling**


**01 Scalability** is an ability of the system to scale by increasing its resources size like upscale its CPU, RAM or storage, resource amount etc.

**Vertical Scaling** - adding more power to resource.

**Horizontal scaling** - increasing amount of the resources.

**Scaling in** - increasing the resources.

**Scaling out** - decreasing the resources.

**Elasticity** is an ability of the system to allocate and deallocate resources dynamically whenever it needed them. 

**Automatic scaling** is an ability to allocate and deallocate resources automatically. In other words it's **elasticity**.

**Agility** means being able to allocate and deallocate resources in a very short time in comparison with on-premises environment.

---

## **02 Robustness**

**Fault Tolerance** is an ability of the system to remain up and running during an either component or a service failures. Most of the time in the cloud all the services have built-in **FT**.

**Disaster Recovery** is a system's ability to recovery from even something is taken down entire region or service and it's a simple way for system to work properly after a disaster due to make a mirror or 2 copies of the same applications in a different regions. 

**High Availability** is an ability of the system to run for very extended periods of time with very little downtime.

---

## **03 Principles of Economies of Scale**

**Key Characteristics** - cost per unit (or service) lowers as the size of the company grows

---

## **CapEx (Capital Expenditure) and OpEx (Operational Expenditure)**

**CapEx (Capital Expenditure)** means bying your own infrastructure when you pay fully for an infrastructure but take economy by paid for maintenance which is only power supply, networking, some hardware replacements and looking after employees.

**04 CapEx Key Characteristics:**

- **Own** infrastructure;
- Big **initial investment**;
- Lots of **maintenance** required:
    - Support staff;
    - Power Supply & Networking;
    - Hardware failures;
    - others

**OpEx (Operational Expenditure)** means paid only for used infrastructure at the moment.

**OpEx Key Characteristics:**

- **Rent** infrastructure;
- **No initial investment**, pay for what you use;
- Minimal **maintenance**;
    - Operations team;

**CapEx vs. OpEx**
**Differences**

|Characteristic|CapEx|OpEx|
|---|---|---|
|Up front cost|Significant|None|
|Ongoing cost|Low|Based on usage|
|Tax Deduction|Over time|Same year|
|Early Termination|No|Anytime|
|Maintenance|Significant|Low|
|Value over time|Lowers|No change|

---

## **04 Consumption-based Pricing Model in the Cloud**

In **Consumption-based Pricing Model in the Cloud** you have multiple pricing components per each service. Additionally the charges are very very granularm if you only use a virtual machine for 20 seconds, you will pay only for 20 seconds of the usage.

**Key Characteristics:**

- No upfront costs;
- No wasted resources;
- Pay for additional resources when needed;
- Stop paying at any time.

---

## **05 IaaS PaaS SaaS**

**IaaS (Infrastructure as a Service)** - customer manages **platform** and then **software** part but **networking**, **hardware** and **virtualization** itself are managed by a **cloud provider**:
- Virtual Machine;
- Virtual Network;
- Managed Disk;

**PaaS (Platform as a Service)** - cunsomer manages **software** which is may be its own application but other are managed by a **cloud provider**:
- SQL;
- App Service;
- Logic Apps;
- Function Apps;

**SaaS (Software as a Service)** - cunsomer only use prepared **software** and avarything are managed by a **cloud provider**:

- Skype;
- Outlook;
- OneDrive;
- other apps;

---

## **06 Public, Private and Hybrid Cloud**

**Public Cloud** is a cloud where cloud provider hosting and managing all the hardware and consumer can't do anything with the cloud hardware;

**Private Cloud** is a cloud hested and managed by resource owner as some organization;

**Hybrid Cloud** is a mixture of Public and Private Clouds usage; 

---

## **07 Geographies, Regions and Zones**

**Data Center** - a physical facility that hosts a group of network servers:
Services:
- SQL;
- Web;
- VM;
Own properties:
- Power;
- Cooling;
- Networking.

**Region**

Key Characteristics:

- Geographical area on the planet;
- One but usually more datacenters connected with low-latency network (<2 milliseconds);
- Location for your services;
- Some services are available only in certain regions;
- Some services are global services, as such are not assigned/deployed in specific region;
- Globally available with 50+ regions;
- Special government regions (US DoD Central, US Gov Virginia, etc.);
- Special partnered regions (China East, China North).

To choose the nearest region to host your service you may use the following link: http://azurespeedtest.azurewebsites.net/

To check the available products by region you may use the following link: https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/

**Availability Zone**

Key Characteristics:

- Regional feature;
- Grouping of physically separate facilities;
- Designed to protect from data center failures;
- If zone goes down others continue working;
- Two service categories;
    - Zonal services (Virtual Machines, Disks, etc.);
    - Zone-redundant services (SQL, Storage, etc.);
- Not all regions are supported;
- Supported region has three or more zones;
- A zone is one or more data centers.

**Region Pair**

Key Characteristics:

- Each region is paired with another region making it a region pair;
- Region pairs are static and cannot be chosen;
- Each pair resides within the same geography;
    - Exception is Brazil South;
- Physical isolation with at least 300 miles distance (when possible);
- Some services have platform-provided replication;
- Planned updates across the pairs;
- Data residency maintained for disaster recovery.

**Geography**

Key Characteristics:

- Discrete market;
- Typically contains two or more regions;
- Ensures data residency, sovereignty, resiliency, and compliance requirements are met;
- Fault tolerant to protect from region wide failures;
- Broken up into areas:
    - Americas;
    - Europe;
    - Asia Pacific;
    - Middle East and Africa;
- Each region belongs only to one Geography.

## **08 Resources, Resource Groups and Resource Management**

**Resources**

Key Characteristics:

- Object used to manage services in Azure;
- Represents service lifecycle;
- Saved as JSON definition.

**Resource Groups**

Key Characteristics:

- Grouping of resources;
- Holds logically related resources;
- Typically organizing by:
    - Type;
    - Lifecycle (app, environment);
    - Department;
    - Billing,
    - Location or
    - combination of those.

**Resource Management**

Key Characteristics:

- Management Layer for all resources and resource groups;
- Unified language;
- Controls access and resources.

**Additional Info**
- Each resource must be in one, and only one resource group
- Resource groups have their own location assigned
- Resources in the resource groups can reside in a different locations
- Resources can be moved between the resource groups
- Resource groups can’t be nested
- Organize based on your organization needs but consider
    - Billing
    - Security and access management
    - Application Lifecycle

---

## **09 Compute Services**

## **Virtualization**

- Emulation of physical machines
- Different virtual hardware configuration per machine/app
- Different operating systems per machine/app
- Total separation of environments
    - file systems,
    - services,
    - ports,
    - middleware,
    - configuration

## **Virtual Machines**

**Key Characteristics:**

- Infrastructure as a Service (IaaS);
- Total control over the operating system and the software;
- Supports marketplace and custom images;
- Best suited for:
    - Custom software requiring custom system configuration;
    - Lift-and-shift scenarios;
    - Can run any application/scenario
    - web apps & web services,
    - databases,
    - desktop applications,
    - jumpboxes,
    - gateways, etc.

## **Virtual Machine Scale Sets**

- Infrastructure as a Service (IaaS);
- Set of identical virtual machines;
- Built-in auto scaling features;
- Designed for manual and auto-scaled workloads like web services,* batch processing, etc.

### **Containers**

- Use host’s operating system;
- Emulate operating system (VMs emulate hardware);
- Lightweight (no O/S):
    - Development Effort;
    - Maintenance;
    - Compute & storage requirements;
- Respond quicker to demand changes;
- Designed for almost any scenario.

## **Azure Container Instances**

- Simplest and fastest way to run a container in Azure;
- Platform as a Service;
- Serverless Containers;
- Designed for:
    - Small and simple web apps/services;
    - Background jobs;
    - Scheduled scripts.

## **Azure Kubernetes Service (AKS)**

- Open-source container orchestration platform;
- Platform as a Service;
- Highly scalable and customizable;
- Designed for high scale container deployments (anything really!).

## **App Service**

- Designed as enterprise grade web application service
- Platform as a Service
- Supports multiple programming languages and containers

## **Azure Functions (Function Apps)**

- Platform as a Service
- Serverless
- Two hosting/pricing models
    - Consumption-based plan
    - Dedicated plan
- Designed for micro/nano-services

## **Summary**

- Virtual Machines (IaaS) - Custom software, custom requirements, very specialized, high degree of control
- VM Scale Sets (IaaS) - Auto-scaled workloads for VMs
- Container Instances (PaaS) - Simple container hosting, easy to start
- Kubernetes Service (PaaS) - Highly scalable and customizable * container hosting platform
- App Services (PaaS) - Web applications, a lot of enterprise web * hosting features, easy to start
- Functions (PaaS) (Function as a Service) (Serverless) - micro/nano-services, excellent consumption-based pricing, easy to start

---

## **10 Networking Services**

**Networking Services** is a category of services allows customers to connect their on-premise and cloud resources but also help with protection and monitoring of the networking of those servcies as well as helping customers with application delivery.

---

## **Azure Networking**

- Connect cloud and on-premises;
- On-premise networking functionality.

---

## **Azure Virtual Network**

- Logically isolated networking components;
- Segmented into one or more subnets;
- Subnets are discrete sections;
- Enable communication of resources with each-other, internet and on-premises;
- Scoped to a single region;
- VNet peering allow cross region communication;
- Isolation, Segmentation, Communication, Filtering, Routing.

---

## **Azure Load Balancer**

- Even traffic distribution;
- Supports both inbound and outbound scenarios;
- High-availability scenarios;
- Both TCP (transmission control protocol) and UDP (user datagram protocol) applications;
- Internal and External traffic;
- Port Forwarding;
- High scale with up to millions of flows.

---

## **VPN Gateway**

- Specific type of virtual network gateway for on-premises to azure traffic over the public internet;

---

## **Application Gateway**

- Web traffic load balancer;
- Web application firewall;
- Redirection;
- Session affinity;
- URL Routing;
- SSL termination.

---

## **Content Delivery Network**

- Define content;
- Minimize latency;
- POP (points of presence) with many locations.

---

# **11 Azure Storage Services**

## **Data Types**
- Structured - Data that can be represented using tables with very strict schema. Each row must follow defined schema. Some tables have defined relationships between them. Typically used in relational databases.
- Semi-structured - Data that can be represented using tables but without strict defined schema. Rows must only have unique key identifier.
- Unstructured - Any files in any format. Like binary files, application files, images, movies, etc.

---

## **Storage Account**
- Group of services which include:
    - blob storage,
    - queue storage,
    - table storage, and
    - file storage;
- Used to store:
    - files,
    - messages, and
    - semi-structured data;
- Highly scalable (up to petabytes of data);
- Highly durable (99.999999999% - 11 nines, up to 16 nines);
- Cheapest per GB storage.

---

## **Blob Storage**
- BLOB – binary large object – file;
- Designed for storage of files of any kind;
- Three storage tiers:
    - Hot – frequently accessed data;
    - Cool – infrequently accessed data (lower availability, high durability);
    - Archive – rarely (if-ever) accessed data.
    
---

## **Queue Storage**
- Storage for small pieces of data (messages);
- Designed for scalable asynchronous processing.

---

## **Table Storage**
- Storage for semi-structured data (NoSQL):
    - No need for foreign joins, foreign keys, relationships or strict schema;
    - Designed for fast access;
- Many programming interfaces and SDKs.

---

## **File Storage**
- Storage for files accessed via shared drive protocols;
- Designed to extend on-premise file shares or implement lift-and-shift scenarios.

---

## **Disk Storage**
- Disk emulation in the cloud;
- Persistent storage for Virtual Machines;
- Different:
    - sizes,
    - types (SSD, HDD)
    - performance tiers.
- Disk can be unmanaged or managed.

# **12 Database Services**

## **Data Types**
- Structured - Data that can be represented using tables with very strict schema. Each row must follow defined schema. Some tables have defined relationships between them. Typically used in relational databases;
- Semi-structured - Data that can be represented using tables but without strict defined schema. Rows must only have unique key identifier;
- Unstructured - Any files in any format. Like binary files, application files, images, movies, etc.

---

## **Cosmos DB**
- Globally distributed NoSQL (semi-structured data) Database service;
- Schema-less;
- Multiple APIs (SQL, MongoDB, Cassandra, Gremlin, Table Storage);
- Designed for:
    - Highly responsive (real time) applications with super low latency responses <10ms;
    - Multi-regional applications.

---

## **SQL Database**
- Relational database service in the cloud (PaaS) (DBaaS - Database as a Service);
- Structured data service defined using schema and relationships;
- Rich Query Capabilities (SQL);
- High-performance, reliable, fully managed and secure database for building - applications.

---

## **Azure SQL product family**
- Azure SQL Database – Reliable relational database based on SQL Server;
- Azure Database for MySQL – Azure SQL version for MySQL database engine;
- Azure Database for PostgreSQL – Azure SQL version for PostgreSQL database engine;
- Azure SQL Managed Instance – Fully fledged SQL Server managed by cloud provider;
- Azure SQL on VM – Fully fledged SQL Server on IaaS;
- Azure SQL DW (Synapse) – Massively Parallel Processing (MPP) version of SQL Server.

---

# **13 Azure Merketplace**

## **Azure Marketplace**
- Think of it like an “Azure Shop” where you purchase services and solutions for the Azure platform;
- Each product is a template which contains one or multiple services;
- Products are delivered by first and third-party vendors;
- Solutions can leverage all service categories like IaaS, PaaS and SaaS.

---

# **14 Azure IoT Services**

## **What is Internet of Things?**
Internet of Things (IoT) is a network of internet connected devices (IoT Devices) embedded in everyday objects enabling sending and receiving data such as settings and telemetry.

## **Azure Iot Hub**

- Managed service for bi-directional communication;
- Platform as a Service (PaaS);
- Highly secure, scalable and reliable;
- Integrates with a lot of Azure Services;
- Programmable SDKs for popular languages (C, C#, Java, Python, Node.js);
- Multiple protocols (HTTPS, AMQP, MQTT).

## **Azure IoT Central**
- IoT App Platform - Software as a Service (SaaS);
- Industry specific app templates;
- No deep technical knowledge required;
- Service for connecting, management and monitoring IoT devices;
- Highly secure, scalable and reliable;
- Built on top of the IoT Hub service and 30+ other services.

## **Azure Sphere**
- Secure end-2-end IoT Solutions;
-   Azure Sphere certified chips (microcontroller units - MCUs);
-   Azure Sphere OS based on Linux;
-   Azure Security Service trusted device-to-cloud communication.

---

# **15 Azure Big Data & Analytics Services**

## **What is Big Data?**

**Big Data** is a field of technology that helps with the extraction, processing and analysis of information that is too large or complex to be dealt with by traditional software.

## **The three V’s rule**

Big data typically has one of the following characteristics:
- Velocity - how fast the data is coming in or how fast we are processing it:
    - Batch;
    - Periodic;
    - Near Real Time;
    - Real Time.
- Volume - how much data we are processing:
    - Megabytes;
    - Gigabyte;
    - Terabytes;
    - Petabytes.
- Variety - how structured/complex the data is:
    - Tables;
    - Databases;
    - Photo, Audio;
    - Video, Social Media.

## **Azure Synapse Analytics**

- Big data analytics platform (PaaS);
- Multiple components:
    - Spark;
    - Synapse SQL:
        - SQL pools (dedicated – pay for provisioned performance);
        - SQL on-demand (ad-hoc – pay for TB processed);
    - Synapse Pipelines (Data Factory – ETL);
    - Studio (unified experience).

## **Azure HDInsight**

- Flexible multi-purpose big data platform (PaaS);
- Multiple technologies supported (Hadoop, Spark, Kafka, HBase, Hive, Storm, Machine Learning).

## **Azure Databricks**
- Big data collaboration platform (PaaS);
- Unified workspace for notebook, cluster, data, access management and collaboration;
- Based on Apache Spark;
- Integrates very well with common Azure data services.

---

# **16 Azure Machine Learning**

**Artificial Intelligence (AI)** is the simulation of human intelligence & capabilities by computer software.

**Machine Learning** is a subcategory of AI where a computer software is “taught” to draw conclusions and make predictions from data.

- Cloud-based platform for creating, managing and publishing machine learning models
- Platform as a Service (PaaS)
- Machine Learning Workspace – top level resource
- Machine Learning Studio – web portal for end-2-end development
- Features
    - Notebooks – using Python and R
    - Automated ML – run multiple algorithms/parameters combinations, choose the best model
    - Designer – graphical interface for no-code development
    - Data & Compute – management of storage and compute resources
    - Pipelines – orchestrate model training, deployment and management tasks

---

# **17 Azure Serverless Computing Services**

**Serverless computing** is cloud-hosted execution environment that allows customers to run their applications in the cloud while completely abstracting underlying infrastructure.

## **Azure Functions**
- Serverless coding platform (Functions as a Service, FaaS);
- Designed for nano-service architectures and event-based applications;
- Scales up and down very quickly;
- Highly scalable;
- Supports popular languages and frameworks (.NET & .NET Core, Java, Node.js, Python, PowerShell, etc.).

## **Azure Logic Apps**
- Serverless enterprise integration service (PaaS);
- 200+ connectors for popular services;
- Designed for orchestration of
    - business processes,
    - integration workflows for applications, data, systems and services;
- No-code solution.

## **Azure Event Grid**
- Fully managed serverless event routing service;
- Uses publish-subscribe model;
- Designed for event-based and near-real time applications;
- Supports dozen of built-in events from most common Azure services.

---

# **18 Azure DevOps Solutions**

**DevOps** is a set of practices that combine both development (Dev) and operations (Ops).

**DevOps** aims to shorten the development life cycle by providing continuous integration and delivery (CI/CD) capabilities while ensuring high quality of deliverables.

## **Azure DevOps**

- Collection of services for building solutions using DevOps practices;
- Services included;
    - Boards – tracking work;
    - Pipelines – building CI/CD workflows (build, test and deploy apps);
    - Repos – code collaboration and versioning with Git;
    - Test Plans – manual and exploratory testing;
    - Artifacts – manage project deliverables;
- Extensible with Marketplace – over 1000 of available apps;
- Evolved from TFS (Team Foundation Server), through VSTS (Visual Studio Team Services).

## **Azure DevTest Labs**

- Service for creation of sandbox environments for developers/testers (PaaS);
- Quick setup of self-managed virtual machines;
- Preconfigured templates for VMs;
- Plenty of additional artifacts (tools, apps, custom actions);
- Lab policies (quotas, sizes, auto-shutdowns);
- Share and automate labs via custom images;
- Premade plugins/API/tools for CI/CD pipeline automation.

---

# **19 Azure Tools**

## **Azure Portal**

- Public web-based interface for management of Azure platform;
- Designed for self-service;
- Customizable;
- Simple tasks.

## **Azure PowerShell**

- PowerShell and module;
- Designed for automation;
- Multi-platform with PowerShell Core;
- Simple to use:
    - Connect-AzAccount – log into Azure;
    - Get-AzResourceGroup – list resource groups;
    - New-AzResourceGroup – create new resource group;
    - New-AzVm – create virtual machine.

## **Azure CLI**

- Command Line Interface for Azure;
- Designed for automation;
- Multi-platform (Python);
- Simple to use:
    - az login – log into Azure;
    - az group list – list resource groups;
    - az group create – create new resource group;
    - az vm create – create virtual machine;
- Native OS terminal scripting.

## **Azure Cloud Shell**

- Cloud-based scripting environment;
- Completely free;
- Supports both Azure PowerShell and Azure CLI;
- Dozen of additional tools;
- Multiple client interfaces:
    - Azure Portal integration (portal.azure.com);
    - Shell Portal (shell.azure.com);
    - Visual Studio Code Extension;
    - Windows Terminal;
    - Azure Mobile App;
    - Microsoft Docs integration.


---

# **20 Azure Advisor**

- Personalized consultant service;
- Designed to provide recommendations and best practices for:
    - Cost (SKU sizes, idle services, reserved instances, etc.);
    - Security (MFA settings, vulnerability settings, agent installations, etc.);
    - Reliability (redundancy settings, soft delete on blobs, etc.);
    - Performance (SKU sizes, SDK versions, IO throttling, etc.);
    - Operational Excellence (service health, subscription limits, etc.);
- Actionable recommendations;
- Free!

---

# **21 Azure Security Groups**

## **Network Security Groups**

- Designed to filter traffic to (inbound) and from (outbound) Azure resources located in - Azure Virtual Network;
- Filtering controlled by rules;
- Ability to have multiple inbound and outbound rules;
- Rules are created by specifying:
    - Source/Destination (IP addresses, service tags, application security groups);
    - Protocol (TCP, UDP, any);
    - Port (or Port Ranges, ex. 3389 – RDP, 22 – SSH, 80 HTTP, 443 HTTPS);
    - Direction (inbound or outbound);
    - Priority (order of evaluation).

## **Application Security Groups**

- Feature that allows grouping of virtual machines located in Azure virtual network;
- Designed to reduce the maintenance effort (assign ASG instead of the explicit IP address).

---

# **22 Azure User-Defined Routes**

**Routing** is a process of finding/selecting a path for traffic in one or across multiple networks.

## **User-defined Routes**
- Custom (user-defined, static) routes (UDRs);
- Designed to override Azure’s default routing or add new routes
- Managed via Azure Route Table resource
- Associated with a zero or more Virtual Network subnets.

---

# **23 Azure Firewall**

**Firewall** is a network security service that monitors and controls incoming and outgoing traffic.

## **Azure Firewall**

- Managed, cloud-based firewall service (PaaS, Firewall as a Service);
- Built-in high availability;
- Highly Scalable;
- Inbound & outbound traffic filtering rules;
- Support for FQDN (Fully Qualified Domain Name), ex. microsoft.com;
- Fully integrated with Azure monitor for logging and analytics.

---

# **24 Azure DDoS Protection**

**DoS (Denial of Service)** - Cyber-attack with intent to cause temporary or indefinite disruption of service.

**DDoS (Distributed Denial of Service)** - DoS attack that is originating from multiple servers.

## **Azure DDoS Protection**

- DDoS protection service in Azure:
- Designed to:
    - Detect malicious traffic and block it while allowing legitimate users to connect;
    - Prevent additional costs for auto-scaling environments;
- Two tiers:
    - Basic – automatically enabled for Azure platform;
    - Standard – additional mitigation & monitoring capabilities for Azure Virtual Network resources.
- Standard tier uses machine learning to analyze traffic patterns for better accuracy.

---

# **25 Azure Identity Services**

## **Identity**

- A user with a username and password;
- Also applications or other servers with secret keys or certificates;
- The fact of being something or someone.

## **Authentication**

The process of verification/assertion of identity

## **Authorization**

The process of ensuring that only authenticated identities get access to the resources for which they have been granted access.

## **Access Management**

The process of controlling, verifying, tracking and managing access to authorized users and applications.

## **Azure Active Directory**

- Identity and Access Management service in Azure;
- Identities management – users, groups, applications;
- Access management – subscriptions, resource groups, roles, role assignments, authentication & authorization settings, etc.
- Used by multiple Microsoft cloud platforms:
    - Azure;
    - Microsoft 365;
    - Office 365;
    - Live.com services (Skype, OneDrive, etc.).

## **Multi-factor Authentication (MFA)**

- Process of authentication using more than one factor (evidence) to prove identity;
- Factor types:
    - Knowledge Factor – “Something you know”, ex. password, pin;
    - Possession Factor – “Something you have”, ex. phone, token, card, key;
    - Physical Characteristic Factor – “Something you are”, ex. fingerprint, voice, face, eye iris;
    - Location Factor – “Somewhere you are”, ex. GPS location;
- Supported by Azure AD by default (simple on-off switch).

---

# **26 Azure Security Center**

## **Identity**
- Centralized/unified infrastructure and platform security management service;
- Natively embedded in Azure services;
- Integrated with Azure Advisor;
- Two tiers:
    - Free (Azure Defender OFF) – included in all Azure services, provides continuous assessments, security score, and actionable security recommendations;
    - Paid (Azure Defender ON) – hybrid security, threat protection alerts, vulnerability scanning, just in time (JIT) VM access, etc.

---

# **27 Azure Key Vault**

## **Azure Key Vault**

- Managed service for securing sensitive information (application/platform) (PaaS);
- Secure storage service for:
    - Keys,
    - Secrets and
    - Certificates;
- Highly integrated with other Azure services (VMs, Logic Apps, Data Factory, Web Apps, etc.);
- Centralization;
- Access monitoring and logging.

---

# **28 Azure Role-Based Access Control**

## **What is a Role?**

Role (role definition) is a collection of actions that the assigned identity will be able to perform.

Role definition is an answer to a question "What can be done?"05 Consumption-based Pricing Model in the Cloud

## **What is a Security Principal?**

Security Principal is an Azure object (identity) that can be assigned to a role (ex. users, groups or applications).

Security Principal assignment is an answer to a question "Who can do it?"

## **What is a Scope?**

Scope is one or more Azure resources that the access applies to.

Scope assignment is an answer to a question "Where can it be done?"

## **Azure Role-based Access Control (RBAC)**

- Authorization system built on Azure Resource Manager (ARM);
- Designed for fine-grained access management of Azure Resources;
- Role assignment is combination of:
    - Role definition – list of permissions like create VM, delete SQL, assign permissions, etc.
    - Security Principal – user, group, service principal and managed identity;
    - Scope – resource, resource groups, subscription, management group;
- Hierarchical:
-   Management Groups > Subscriptions > Resource Groups > Resources;
- Built-in and Custom roles are supported.

---

# **29 Azure Resource Lock**

## **What is an Azure Resource Lock?**

- Designed to prevent accidental deletion and/or modification;
- Used in conjunction with RBAC;
- Two types of locks:
    - Read-only (ReadOnly) – only read actions are allowed;
    - Delete (CanNotDelete) – all actions except delete are allowed;
- Scopes are hierarchical (inherited):
    - Subscriptions > Resource Groups > Resources;
- Management Groups can’t be locked;
- Only Owner and User Access Administrator roles can manage locks (built-in roles).

---

# **30 Azure Resource Tags**

## **Azure Resource Tags**

- Tags are simple Name (key) - Value pairs;
- Designed to help with organization of Azure resources;
- Used for resource governance, security, operations management, cost management, automation, etc.
- Typical tagging strategies:
    - Functional – mark by function ( ex: environment = production );
    - Classification – mark by policies used ( ex: classification = restricted );
    - Finance/Accounting – mark for billing purposes ( ex: department = finance );
    - Partnership – mark by association of users/groups ( ex: owner = adam );
- Applicable for resources, resource groups and subscriptions;
- NOT inherited by default.

---

# **31 Azure Policy**

## **Azure Policy**

- Designed to help with resource governance, security, compliance, cost management, etc.;
- Policies focus on resource properties (RBAC focused on user actions);
- Policy definition – Defines what should happen;
    - Define the condition (if/else) and the effect (deny, audit, append, modify, etc.);
    - Examples include allowed resource types, allowed locations, allowed SKUs, inherit resource tags;
- Built-in and custom policies are supported;
- Policy initiative – a group of policy definitions;
- Policy assignment – assignment of a policy definition/initiative to a scope:
    - Scopes can be assigned to:
        - management groups,
        - subscriptions,
        - resource groups, and
        - resources;
- Policies allow for exclusions of scopes;
- Checked during resource creation or updates and existing ones with remediation tasks.

# **32 Azure Blueprints**

## **Azure Blueprints**

- Package of various Azure components (artifacts);
    - Resource Groups;
    - ARM Templates;
    - Policy Assignments;
    - Role Assignments;
- Centralized storage for organizationally approved design patterns;
- Blueprint definition – describing what should happen (reusable package);
- Blueprint assignment – describing where it should happen (package deployment).

---

# **33 Cloud Adoption Framework for Azure**

## **Cloud adoption**

**Cloud adoption** is a strategic move by an organization to leverage cloud in their business.

## **Cloud Adoption Framework**

Cloud Adoption Framework for Azure is a set of:

- tools,
- best practices,
- guidelines and
- documentation.

prepared by Microsoft to help companies with their cloud adoption journey.

## **Strategy**

### **1. Understand your motivation**

- Answer the question WHY MOVE?
- Common Motivation Triggers include:
    - Migration:
        - Cost Savings on infrastructure;
        - Reduction in complexity;
        - Operation optimization;
        - Increased business agility;
    - Innovation:
        - Reaching a global scale;
        - Customer experience improvements;
        - Transformation of products or services;
        - Market disruption.

### **2. Business Outcome**

- Answer the question WHAT TO MEASURE?
- Defined, concise and observable outcome captured by a specific measure, for example:
    - Increase in revenue;
    - Increase in profit;
    - Cost reduction;
    - Global access to customers;
    - Reaching new markets.

### **3. Business Justification**

- Answer the question WHAT’S MY RETURN ON INVESTMENT?
- Develop a business case to validate the financial model that supports your motivations and outcomes;
- Tools that support this process are:
    - Azure TCO (Total Cost of Ownership) calculator - estimate current on-prem costs;
    - Azure Pricing Calculator - estimate future Azure costs;
    - Azure Cost Management - see current Azure costs.

### **4. First Project**

- Choose first project to validate your strategy (Proof of concept - POC) based on:
    - Business Criteria:
        - Currently operating;
        - Dedicated owner;
        - Strong motivation to move;
    - Technical Criteria:
        - Minimum dependencies and assets;

## **Plan**

1. Digital Estate (INVENTORY OF ASSETS):
    - Review current landscape and list all projects/solutions (digital assets);
    - Choose one of the five (5) R’s of rationalization:
        - Rehost - move as is; typically into containers or IaaS (virtual machines);
        - Refactor - make small code changes and move to PaaS (ex. Azure SQL, Azure App Service, etc.);
        - Rearchitect - make complex code changes to introduce new features or fix incompatible apps;
        - Rebuild - create a new application using cloud first design;
        - Replace - review available SaaS solutions and replace legacy or unneeded applications;
2. Initial Organization Alignment:
    - Align people so they will support your adoption plan;
    - Map people to capabilities;
3. Skills Readiness Plan:
    - Review current skills and address the gaps;
4. Cloud Adoption Plan - combine everything from steps 1 to 3 into a single cloud adoption plan.

## **Ready**

1. Azure Setup Guide - Review the Azure setup guide to become familiar with the tools and approaches you need to use to create a landing zone.
2. Azure Landing Zone - Choose an appropriate Azure Subscription type that best suits your needs and establish an initial Azure environment.
3. Extend Landing Zone - Expand the initial landing zone to fit your business needs.
4. Best Practices - Review everything and ensure best practices are followed.

## **Adopt**

### **Migrate**

1. First Migration - migrate your first application to familiarize yourself with the cloud, guidelines and tools;
2. Migration Scenarios - review and prepare migration scenarios/guidelines for your company:
    - Virtual Machines - Linux, Windows, etc.
    - Apps - Java, .NET, NodeJS web apps, etc.
    - Data - SQL Server, PostreSQL, File Servers, etc.
    - Other - VMware, Azure Stack, etc.
3. Best Practices - address common migration needs through the application of consistent best practices;
4. Process Improvements - important part of this process heavy activity is to identify bottlenecks and improve with every migration.

### **Innovate**

1. Business Value Consensus (VALUE TO STRATEGY):
    1. Create hypothetical customer need;
    2. Decide on solution that solves it;
    3. Map this to your strategy.
2. Innovation Guide (TOOLS) - choose available Azure tools that will help your build this application;
3. Best Practices - verify that best practices are followed for all tools in the toolchain;
4. Process Improvements - gather feedback from the users and the customers to improve architectural decisions and future products.

## **Govern & Manage**

1. Define governance solutions - Choose solutions to maintain compliance, security and ensure total control of the environment:
    - Those solutions should focus to:
        - Address Business Needs;
        - Provide Agility;
        - Control Risks.
2. Manage cloud environment (CLOUD OPERATIONS) - Hand over solutions and environment to cloud operations team for maintenance. Team should ensure that stability and costs are always in perfect balance to meet business commitments. Team should allow environment to grow, evolve and adapt to changing business needs.

## **Organize**

Ensure that everyone knows what to do and when to do it for every stage in this process. One of the ways to achieve this is via RACI (Responsible, Accountable, Consulted, and Informed) matrix.

---

# **34 Azure Core Tenets of Security, Privacy and Compliance**

## **Azure Core Tenets of Security, Privacy and Compliance (Trust Center, DPA, OST, and more)**

|Document/Website|Info|Offers|Audience|
|---|---|---|---|
|Microsoft Privacy Statement|Collection, Purpose and Usage of Personal Data|All Microsoft offers including services, applications, websites, software, servers, devices|Everyone - end customers or companies|
|Online Services Terms (OST)|Licensing Terms (legal agreement) - usage rights about Azure services. What can be done and what is forbidden.|Microsoft Online Services like Azure, Microsoft 365 services, Bing Maps, etc.|Organizations - legal teams|
|Data Protection Addendum|Appending to OST describing obligations by both parties (Microsoft and you) with regards to the processing of customer and personal data|Microsoft Online Services like Azure, Microsoft 365 services, Bing Maps, etc.|Organizations - legal teams, security teams|
|Trust Center|One stop shop web portal for everything related to security, compliance, privacy, policies, best practices, etc.|Microsoft Online Services like Azure, Microsoft 365 services, Bing Maps, etc.|Organizations - legal teams, security teams, business managers, administrators|
|Azure Compliance Documentation|Web portal focusing on compliance offerings in Azure, simmilar to the trust center but narrowed down|Azure|Organizations - legal teams, security teams, business managers, Azure administrators|

## **Azure Sovereign Regions**

Azure Sovereign Regions provide Azure services in markets with very strict regulatory requirements

- Azure Government designed for the US government:
    - Separate instance of Azure (lifecycle, services, portal, etc.);
    - Physically isolated from other Azure regions;
    - Only autorized scanned personel can get access;
- Azure China designed for the Chinese market:
    - Separate instance of Azure (lifecycle, services, portal, etc.);
    - Physically isolated from other Azure regions;
    - Operated by a Chinese telecom company called 21Vianet.

---

# **35 Azure Cost Affecting Factors**

## **Cost Affecting Factors**

- Base Cost:
    - Resource Types – All Azure services (resources) have resource-specific pricing models. Typically consisting of one or more metrics;
    - Services – Azure specific offers (Enterprise, Web Direct, CSP, etc.) have different cost and billing components like prepaids, billing cycles, - discounts, etc.;
    - Location – running Azure services vary between Azure regions;
    - Bandwidth – network traffic when uploading (inbound/ingress) data to Azure or downloading (outbound/egress) from Azure;
- Savings:
    - Reserved Instances;
    - Hybrid Benefits.

---

# **36 Azure Cost Reduction Methods**

## **Azure Reservations**

Purchase Azure services for 1 or 3 years in advance with a significant discounts:

- Reserved instances – Azure Virtual Machines;
- Reserved capacity – Azure Storage, SQL Database vCores, Databricks DBUs, Cosmos DB RUs;
- Software plans – Red Hat, Red Hat OpenShift, SUSE Linux, etc.;
- Reservations are made for 1 or 3 years.

## **Azure Spot VMs**

Purchase unused Virtual Machine capacity for significant discount:

- How it works:
    - Significant dicount for Azure VMs;
    - Capacity can be taken away at any time;
    - Customer can set maximum price after discount to keep or evict the machine;
- Best for interruptable workloads (batch processing, dev/test environments, large compute workloads, non-critical tasks, etc.).

## **Hybrid use Benefit**

Use existing licenses in the cloud:

- Use existing licenses in the Azure:
    - Windows Server:
        - Azure VM;
    - RedHat:
        - Azure VM;
    - SUSE Linux:
        - Azure VM;
    - SQL Server:
        - Azure SQL Database;
        - Azure SQL Managed Instance;
        - Azure SQL Server on VM;
        - Azure Data Factory SQL Server Integration Services.


## **Tools**

- Pricing calculator – estimate the cost of Azure services:
    - Select service;
    - Adjust parameters (usage);
    - View the price;
- Total Cost of Ownership (TCO) calculator – estimate and compare the cost of running workloads in datacenter versus Azure:
    - Define your workloads;
    - Adjust assumptions;
    - View the report.

---

# **37 Azure Cost Management**

## **Azure Cost Management**

- A centralized service for reporting usage and billing of Azure environment
- Self-service cost exploration capabilities
- Budgets & alerts
- Cost recommendations
- |Automated exports

## **Minimizing Costs in Azure**

1. Azure Pricing Calculator to choose the low-cost region:
    - Good latency;
    - All required services are available;
    - Data sovereignty/compliance requirements;
2. Hybrid use benefit and Azure Reservations;
3. Azure Cost Management monitoring, budgets, alerts and recommendations;
4. Understand service lifecycle and automate environments;
5. Use autoscaling features to your advantage;
6. Azure Monitor to find and scale down underutilized resources;
7. Use tags & policies for effective governance.

---

# **38 Azure SLA and Composite SLA**

## **SLA**

Service Level Agreement (SLA) is a formal agreement between a service provider and a customer.

SLA is a promise of a service’s availability (uptime & connectivity). Availability is a measure of time that a service remains operational.

- Each Service has its own SLA;
- Ranges from 99% to 99.999%;
- Free services typically don’t have an SLA;
- Broken SLA means service credit return (discount).

|SLA|Monthly Downtime|
|---|---|
|99%|7h 18m 17s|
|99.5%|3h 39m 8s|
|99.9%|43m 49s||
|99.95%|21m 54s|
|99.99%|4m 22s|
|99.999%|26s|

## **Formulas**

### **Logical AND - adding dependency:**

Availability of S1 AND S2 = Availability(S1) * Availability(S2)

### **Scenario - Azure website with SQL backend db:**

- Availability = Availability(web) app * Availability(sql);
- Availability = 99.95% * 99.95%;
- Availability = 0.9995 * 0.9995;
- Availability = 0.99900025;
- Availability ~ 99.9%.

### **Logical OR - adding redundancy:**

Availability of S1 OR S2 = 100% - ( Unvailability(S1) * Unvailability(S2) )

### **Scenario - Two redundant web apps behind a load balancer**

- Availability(both-web) = 100% - ( Unvailability(web1) * Unvailability(web2) );
- Availability(both-web) = 100% - ( 0.05% * 0.05% );
- Availability(both-web) = 1 – ( 0.0005 * 0.0005 );
- Availability(both-web) = 1 – 0.00000025;
- Availability(both-web) = 0.99999975;
- Availability(both-web) ~ 99.9999%.

## **Key Items**

- Formal agreement between Microsoft & the customer;
- Calculated as a percentage of service availability (uptime & connetivity) (a promise);
- Breaking the SLA provides a discount from the final monthly bill (Service Credit);
- Higher tier services offer better SLAs;
- Free services typically have no SLA (0% SLA);
- Preview services have no SLA;
- Composite SLA is a combined SLA of all application components.
 
 ---

# **39 Azure Service Lifecycle**

## **Service Lifecycle**

- Every service in Azure follows its own service lifecycle;
- Public preview is a ‘beta’ stage of the service available to general public use;
- Features can also be in preview stages;
- Designed for testing, not production solutions;
- General availability is a ‘production’ release of the service.

## **Public Preview Key Info**

- No SLA;
- Some services have no support coverage;
- Limited region availability;
- Limited functionality;
- Pricing changes;
- Direction changes;
- Azure Portal Previews (https://preview.portal.azure.com).
