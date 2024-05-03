import React, { useState } from "react";
import Swal from "sweetalert2";
import { createDriver } from "../../services/DriversService";

const Add = ({ setIsAdding }) => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");

  const handleAdd = async (e) => {
    e.preventDefault();

    if (!name || !email) {
      return Swal.fire({
        icon: "error",
        title: "Error!",
        text: "All fields are required.",
        showConfirmButton: true,
      });
    }

    setIsAdding(false);

    await createDriver({ name, email }); // Create the driver

    Swal.fire({
      icon: "success",
      title: "Added!",
      text: `${name}'s data has been Added.`,
      showConfirmButton: false,
      timer: 1500,
    });
  };

  return (
    <div className="small-container">
      <form onSubmit={handleAdd}>
        <h1>Add Driver</h1>
        <label htmlFor="name">Name</label>
        <input
          id="name"
          type="text"
          name="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          name="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <div style={{ marginTop: "30px" }}>
          <input type="submit" value="Add" />
          <input
            style={{ marginLeft: "12px" }}
            className="muted-button"
            type="button"
            value="Cancel"
            onClick={() => setIsAdding(false)}
          />
        </div>
      </form>
    </div>
  );
};

export default Add;
