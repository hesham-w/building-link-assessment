export async function getAllDrivers() {
  const response = await fetch(`${this.baseUrl}/driver`);
  const data = await response.json();
  return data;
}

export async function getDriverById(driverId) {
  const response = await fetch(`${this.baseUrl}/driver/${driverId}`);
  const data = await response.json();
  return data;
}

export async function createDriver(driverData) {
  const response = await fetch(`${this.baseUrl}/driver`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(driverData),
  });
  const data = await response.json();
  return data;
}

export async function updateDriver(driverId, driverData) {
  const response = await fetch(`${this.baseUrl}/driver/${driverId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(driverData),
  });
  const data = await response.json();
  return data;
}

export async function deleteDriver(driverId) {
  const response = await fetch(`${this.baseUrl}/driver/${driverId}`, {
    method: "DELETE",
  });
  const data = await response.json();
  return data;
}
