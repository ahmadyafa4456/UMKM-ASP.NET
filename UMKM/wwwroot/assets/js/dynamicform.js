const input_group = document.getElementById("input-group");
const addbtn = document.getElementById("addbtn");
function addForm() {
    const addrow = `<div class="form-group p-3 border border-1 rounded-2 mb-3">
                              <div class="d-flex justify-content-between">
                                <h3>Bahan</h3>
                                <a class="btn btn-danger rounded-4 ms-2 remove-btn" id="">
                                  <i class="fas fa-times"></i>
                                </a>
                              </div>
                              <label asp-for="nama">Nama Bahan</label>
                                    <input type="text"
                                           class="form-control"
                                           asp-for="nama"
                                           placeholder="Nama Bahan" />
                                    <span asp-validation-for="nama" class="text-danger"></span>
                                    <label asp-for="jumlah">Jumlah</label>
                                    <input type="number"
                                           class="form-control"
                                           asp-for="jumlah"
                                           placeholder="Jumlah Bahan" />
                                    <span asp-validation-for="jumlah" class="text-danger"></span>
                                    <label asp-for="harga">Harga</label>
                                    <input type="number"
                                           class="form-control"
                                           asp-for="harga"
                                           placeholder="Harga Bahan" />
                                    <span asp-validation-for="harga" class="text-danger"></span>
                            </div>`;
    input_group.insertAdjacentHTML("beforeend", addrow);
}
input_group.addEventListener("click", function (e) {
    if (
        e.target.classList.contains("remove-btn") ||
        e.target.closest(".remove-btn")
    ) {
        const formGroup = e.target.closest(".form-group");
        if (formGroup) formGroup.remove();
    }
});
addbtn.addEventListener("click", addForm);
