# **Azure Cloud Theory**

|AZ-900 Domain Area|Weight|
|---|---|
|1. Describe cloud concepts|25-30%|
|2. Describe Azure architecture and services|35-40%|
|3. Describe Azure management and governance|30-35%|



## **Theory 01: DR, HA and FT (Domain Area 1)**

**DR (Disaster Recovery)** - DR comes due to event is calamity so when there some kind of a disaster like flood or earthquake. Then one or two machines does not get affected but the whole infrastructure of the whole region gets affected. So in that kind of scenario we are not talking about just recovering the machines and applications. It isn't just a coating about the machines but it also recovering the infrastructure and how we can replace the current infrastructure with the infrastructure somewhere else. It's also about people, logistics, and something big which cost and recovery time are tremendously high.

**FL (Fault Tolerance)** - If you can immidiately make a switch to another machine, it is fault tolerance. If you cannot afford any downtime then you need to have such kind of an architecture wherein you would be replacing or synchronizing or mirroring all the applications, their state, the database and when one of the machines cuts off you will be able to switch its responsibilities to another machine. Fault Tolerance requires a cost but not so high as a **Disaster Recovery**.

**HA (High Availability)** - It means that the machines are available and we can bring them up at any time but we don't have to keep running them, we don't have to keep synchronization or mirroring them. So **HA** is when you can afford downtime but there is the way to go ahead.

**Base things** for each statements is **Cost** and **Recovery Time**.

||Cost|Recovery Time|
|---|---|---|
|DR (Disaster Recovery)|High|High|
|FL (Fault Tolerance)|High|Low|
|HA (High Availability)|Low|High|

## **Theory 02: Agility, Scalability and Flexibility**

**Agility** - It means to change quickly, to respond quickly so you don't need to build assembly locally and use FTP to send files to server and make any other slow actions to make a deployment. We just should be able to do it quickly as soon as possible.

Is you need to do testing, create a network right now itself you should be do taht in an interval about between minutes to an hour.

**Scalability** - It means when your workload increases right now so as the load go up you can to go ahead and increase servers.

**Flexibility** - It takes scalability one step ahead. If load go up you would like to ramp your resources up but when load goes down you need to ramp your consumable resources down. 


## **Question 8: Fault tolerance questions (Domain Area 1)**

If one of data centers goes offline for an extended period and you can make switch to another data center it's **Fault Tolerance**.

## **Question 9: How is high availability different from load balancing ?**

Load balancing is elasticity of a resources when we can change resource consumption by time or by specific conditions.

## **Question 10: In what scenarios we should use DR,FT,latency and Scalability?**

**Disaster Recovery** - a cloud service that can be recovered after it occurs;

**Fault Tolerance** - a cloud service that remains available after it occurs;

**Low Latency** - a cloud service that can be accessed quickly to the internet;

**Dynamic Scalability** - a cloud service that performs quickly when it increases.

## **Question 11: Data center and Availability Zones.**
