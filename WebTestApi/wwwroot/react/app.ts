// Модальное окно и обработка протоколов встроены в компонент
import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { Modal, Button, Form } from "react-bootstrap";

interface Forklift {
    id: number;
    brand: string;
    number: string;
    capacity: number;
    isActive: boolean;
    modifiedAt: string;
    modifiedBy: string;
}

interface ForkliftFault {
    id: number;
    problemDetectedAt: string;
    problemResolvedAt?: string;
    downtime: string;
    reason: string;
}

const App = () => {
    const [forklifts, setForklifts] = useState<Forklift[]>([]);
    const [token, setToken] = useState(localStorage.getItem("jwt") || "");
    const [search, setSearch] = useState("");
    const [selectedForklift, setSelectedForklift] = useState<Forklift | null>(null);
    const [editMode, setEditMode] = useState(false);
    const [form, setForm] = useState({ brand: "", number: "", capacity: 1, isActive: true });
    const [faults, setFaults] = useState<ForkliftFault[]>([]);
    const [showModal, setShowModal] = useState(false);
    const [editFaultId, setEditFaultId] = useState<number | null>(null);
    const [faultForm, setFaultForm] = useState({
        problemDetectedAt: "",
        problemResolvedAt: "",
        reason: "",
    });

    const fetchForklifts = async () => {
        const res = await fetch("/fork/lift", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({ searchPattern: search, maxCount: 100 }),
        });
        const data = await res.json();
        setForklifts(data);
    };

    const fetchFaults = async (forkliftId: number) => {
        const res = await fetch("/fork/lift/fault", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({ ownerId: forkliftId, maxCount: 100 }),
        });
        const data = await res.json();
        setFaults(data);
    };

    const handleSaveFault = async () => {
        if (!selectedForklift) return;
        const method = editFaultId ? "PATCH" : "PUT";
        const url = editFaultId
            ? `/fork/lift/fault/${editFaultId}`
            : `/fork/lift/fault/${selectedForklift.id}`;
        const res = await fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                ...faultForm,
                problemDetectedAt: new Date(faultForm.problemDetectedAt).toISOString(),
                problemResolvedAt: faultForm.problemResolvedAt ? new Date(faultForm.problemResolvedAt).toISOString() : null,
            }),
        });
        if (res.ok) {
            fetchFaults(selectedForklift.id);
            setShowModal(false);
            setEditFaultId(null);
            setFaultForm({ problemDetectedAt: "", problemResolvedAt: "", reason: "" });
        } else {
            alert("Ошибка при сохранении протокола");
        }
    };

    const handleEditFault = (fault: ForkliftFault) => {
        setEditFaultId(fault.id);
        setFaultForm({
            problemDetectedAt: fault.problemDetectedAt.slice(0, 16),
            problemResolvedAt: fault.problemResolvedAt?.slice(0, 16) || "",
            reason: fault.reason,
        });
        setShowModal(true);
    };

    const handleDeleteFault = async (id: number) => {
        if (!window.confirm("Удалить протокол?")) return;
        const res = await fetch(`/fork/lift/fault/${id}`, {
            method: "DELETE",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        if (res.ok && selectedForklift) fetchFaults(selectedForklift.id);
        else alert("Ошибка удаления");
    };

    const handleOpenModal = () => {
        setEditFaultId(null);
        setFaultForm({ problemDetectedAt: "", problemResolvedAt: "", reason: "" });
        setShowModal(true);
    };

    const handleCloseModal = () => setShowModal(false);

    return (
        <div>
        { selectedForklift && (
            <Button className= "btn btn-sm btn-primary mt-3" onClick = { handleOpenModal } >
                Добавить протокол
                    < /Button>
      )}

<Modal show={ showModal } onHide = { handleCloseModal } >
    <Modal.Header closeButton >
    <Modal.Title>{ editFaultId? "Редактировать": "Добавить" } протокол < /Modal.Title>
        < /Modal.Header>
        < Modal.Body >
        <Form>
        <Form.Group>
        <Form.Label>Дата < /Form.Label>
        < Form.Control type = "datetime-local" value = { faultForm.problemDetectedAt } onChange = {(e) => setFaultForm({ ...faultForm, problemDetectedAt: e.target.value })} />
            < /Form.Group>
            < Form.Group >
            <Form.Label>Закрыто < /Form.Label>
            < Form.Control type = "datetime-local" value = { faultForm.problemResolvedAt } onChange = {(e) => setFaultForm({ ...faultForm, problemResolvedAt: e.target.value })} />
                < /Form.Group>
                < Form.Group >
                <Form.Label>Причина < /Form.Label>
                < Form.Control type = "text" value = { faultForm.reason } onChange = {(e) => setFaultForm({ ...faultForm, reason: e.target.value })} />
                    < /Form.Group>
                    < /Form>
                    < /Modal.Body>
                    < Modal.Footer >
                    <Button variant="secondary" onClick = { handleCloseModal } > Отмена < /Button>
                        < Button variant = "primary" onClick = { handleSaveFault } > Сохранить < /Button>
                            < /Modal.Footer>
                            < /Modal>

{
    selectedForklift && (
        <div className="mt-4" >
            <h5>Протоколы погрузчика №{ selectedForklift.number } </h5>
                < table className = "table table-bordered table-sm" >
                    <thead>
                    <tr>
                    <th>№</th>
                        < th > Дата < /th>
                        < th > Закрыто < /th>
                        < th > Простой < /th>
                        < th > Причина < /th>
                        < th > Действия < /th>
                        < /tr>
                        < /thead>
                        <tbody>
    {
        faults.map(f => (
            <tr key= { f.id } >
            <td>{ f.id } < /td>
            < td > { new Date(f.problemDetectedAt).toLocaleString() } < /td>
            < td > { f.problemResolvedAt ? new Date(f.problemResolvedAt).toLocaleString() : '-' } < /td>
            < td > { f.downtime } < /td>
            < td > { f.reason } < /td>
            < td >
            <Button variant="outline-secondary" size = "sm" onClick = {() => handleEditFault(f)}>
                      ✎
    </Button>{' '}
        < Button variant = "outline-danger" size = "sm" onClick = {() => handleDeleteFault(f.id)
}>
                      ✖
</Button>
    < /td>
    < /tr>
              ))}
</tbody>
    < /table>
    < /div>
      )}
</div>
  );
};

export default App;
