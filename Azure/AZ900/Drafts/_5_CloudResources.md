# **Azure Cloud Resources**

|AZ-900 Domain Area|Weight|
|---|---|
|1. Describe cloud concepts|25-30%|
|2. Describe Azure architecture and services|35-40%|
|3. Describe Azure management and governance|30-35%|

## **Question 20: Resource group questions. (Domain Area 2)**

**Resource Group** is a container that holds related resources for an Azure Solution.

Azure resources can access other resources not only in the same resource group but also in other resource groups because you can move any resource to anothjer resource group but you need to update new resorce Id where it was used.

If you delete a resource group, all the resources in the resource group will be deleted.

A resource group can contain resources from multiple Azure regions.

## **Question 21: Can AZ protect Data centre failures. (Domain Area 2)**

If you need to identify the type of failure for which an Azure availability zone can used to protect access to Azure services it should be an Azure Data Center Failure.

## **Question 22: Does Azure policies do deployment. (Domain Area 2)**

Azure Resource Manager (ARM) provides a common platform for deploying objects to a cloud infrastructure and for implementing consistency across the Azure environment.

## **Question 23: How can we automate creation of Azure resources. (Domain Area 2)**

If your company have several business units and each business unit requires several different Azure Resources for daily operation, all the business units require the same type of Azure Resources, and you need to automate the creationof the Azure Resources, you should take the solution of using **Azure Resource Manager (ARM) templates**.

## **Question 24: Delegate permissions to Azure resources. (Domain Area 2)**

When you need to delegate permissions to several Azure virtual machines simultaneously, you must deploy the Azure virtual machines **to the same Resource Group**.