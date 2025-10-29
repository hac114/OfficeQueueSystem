// src/services/api.js
const API_BASE_URL = "http://localhost:5000/api";

export const apiService = {
  getServiceTypes: async () => {
    console.log("📡 Richiedendo servizi dal backend...");
    const response = await fetch(`${API_BASE_URL}/ServiceTypes`);
    const data = await response.json();
    console.log("📦 Servizi ricevuti:", data);
    return data;
  },

  createTicket: async (serviceTag, customerName) => {
    console.log("🎫 Creazione ticket - Parametri ricevuti:", {
      serviceTag,
      customerName,
      tipoServiceTag: typeof serviceTag,
    });

    const requestBody = {
      serviceType: serviceTag, // ⬅️ CAMBIATO: serviceType invece di serviceTypeId
      customerName: customerName || "Cliente",
    };

    console.log("📤 Invio al backend:", JSON.stringify(requestBody));

    try {
      const response = await fetch(`${API_BASE_URL}/Tickets`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });

      console.log("📥 Status risposta:", response.status);

      if (!response.ok) {
        const errorText = await response.text();
        console.error("❌ Errore backend:", errorText);
        throw new Error(`Errore ${response.status}: ${errorText}`);
      }

      const result = await response.json();
      console.log("✅ Ticket creato con successo:", result);
      return result;
    } catch (error) {
      console.error("❌ Errore nella chiamata API:", error);
      throw error;
    }
  },
};
