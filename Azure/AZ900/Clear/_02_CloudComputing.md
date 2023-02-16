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

**Automactic scaling** is an ability to allocate and deallocate resources automatically. In other words it's **elasticity**.

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

## **05 Consumption-based Pricing Model in the Cloud**

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
