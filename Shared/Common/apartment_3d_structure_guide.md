# Apartment 3D Visualization JSON Structure Guide

This guide explains the structure of the JSON data needed for the 3D apartment visualization feature.

## Overview

The 3D visualization requires a specific hierarchical data structure with buildings, floors, apartments, and tenants. The visualization will automatically render buildings with multiple floors, apartments on each floor, and indicate occupancy status.

## JSON Structure

The JSON follows the standard backend-v5 response format with the following hierarchy:

1. **Building** - The top-level structure representing an entire apartment building
2. **Floor** - Each building contains multiple floors
3. **Apartment** - Each floor contains multiple apartment units
4. **Tenant** - Each apartment may have tenants (indicating occupancy)
5. **Amenity** - Buildings may have amenities (pools, gyms, etc.)

## Field Mapping

### Building Fields
- `BuildingId` - Unique identifier for the building
- `BuildingName` - Display name of the building
- `BuildingCode` - Short code for the building
- `Description` - Detailed description of the building

### Floor Fields
- `FloorId` - Unique identifier for the floor
- `FloorNumber` - Numeric floor number (1, 2, 3, etc.)
- `BuildingId` - Reference to the parent building

### Apartment Fields
- `ApartmentId` - Unique identifier for the apartment
- `ApartmentNumber` - Apartment number (101, 102, etc.)
- `UnitType` - Type of apartment (Studio, 1BR, 2BR, etc.)
- `FloorId` - Reference to the parent floor

### Tenant Fields
- `TenantId` - Unique identifier for the tenant
- `FirstName` - Tenant's first name
- `LastName` - Tenant's last name
- `ApartmentId` - Reference to the apartment they occupy

### Amenity Fields
- `AmenityId` - Unique identifier for the amenity
- `AmenityName` - Name of the amenity (Swimming Pool, Gym, etc.)
- `BuildingId` - Reference to the building with this amenity

## 3D Visualization Features

The 3D visualization will render:

1. **Buildings** - Shown as 3D structures with multiple floors
2. **Floors** - Can be viewed individually using the floor slicer
3. **Apartments** - Shown as units on each floor with color-coding for occupancy
4. **Occupancy Status** - Color-coded based on occupancy levels:
   - High Occupancy (Green)
   - Medium Occupancy (Yellow)
   - Low Occupancy (Red)
   - Vacant (Gray)

## Example JSON Structure

```json
{
  "Building": {
    "Id": "BLDG-001",
    "Name": "Sunset Towers",
    "Code": "ST-001",
    "Description": "Luxury apartment building with modern amenities",
    "Floors": [
      {
        "Id": "FL-001",
        "Number": 1,
        "Apartments": [
          {
            "Id": "APT-101",
            "Number": "101",
            "UnitType": "2BR",
            "Tenants": [
              {
                "Id": "TNT-001",
                "FirstName": "John",
                "LastName": "Smith"
              }
            ]
          },
          {
            "Id": "APT-102",
            "Number": "102",
            "UnitType": "1BR",
            "Tenants": []
          }
        ]
      },
      {
        "Id": "FL-002",
        "Number": 2,
        "Apartments": [
          {
            "Id": "APT-201",
            "Number": "201",
            "UnitType": "3BR",
            "Tenants": []
          }
        ]
      }
    ],
    "Amenities": [
      {
        "Id": "AMN-001",
        "Name": "Swimming Pool"
      }
    ]
  }
}
```

## Creating New Records

When creating new records for 3D visualization:

1. Ensure the `ModuleConfigName` is exactly "Apartment Inventory Management"
2. Maintain the proper hierarchy (Building → Floor → Apartment → Tenant)
3. Include all required fields for each entity type
4. Ensure IDs are unique and references are consistent

## Visualization Controls

The 3D view provides several controls:
- Reset View - Returns to the default camera position
- Show All Floors - Shows the complete building
- Slice Building - Shows floors up to a selected level
- Floor Slider - Select which floors to display

## Best Practices

1. Use consistent naming conventions for IDs
2. Ensure floor numbers are sequential integers
3. Provide descriptive names for buildings and amenities
4. Include apartment unit types to enable proper visualization
5. Maintain proper parent-child relationships in the data structure

For more details on implementation, refer to the building-visualizer.component.ts file in the frontend codebase.
